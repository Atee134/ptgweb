/// <reference types="babylonjs"/>
// tslint:disable-next-line:no-reference
/// <reference path="./extensions/babylon.dynamicTerrain.d.ts"/>

import './extensions/babylon.dynamicTerrain.js';

import { Injectable } from '@angular/core';
import { GameInitializerService } from './game-initializer.service';
import { SignalRService } from 'src/app/shared/signalr.service';
import { MapLoadedMessage, LocationDto, LocationChangedMessage } from '../_models/generatedDtos.js';
import { Game } from './interfaces/game.js';
import { Player } from './interfaces/player.js';

@Injectable({
  providedIn: 'root'
})
export class GameManagerService {
  private players: Player[];
  private ownLocation: LocationDto;
  private sessionId: string;
  private game: Game;
  private cameraAltitudeOffset = 3;
  private terrain: BABYLON.DynamicTerrain;

  private terrainChunkRequestThreshold = 28;
  private terrainChunkSizeX: number;
  private terrainChunkSizeZ: number;
  private terrainChunkHalfSizeX: number;
  private terrainChunkHalfSizeZ: number;

  private currentChunkCoords = new BABYLON.Vector2(0, 0);
  private requestedChunks: BABYLON.Vector2[] = [];

  constructor(private gameInitializerService: GameInitializerService, private signalrService: SignalRService) { }

  public startGame(sessionId: string, terrainDataId: string, canvas: HTMLCanvasElement) {
    this.sessionId = sessionId;
    this.subscribeToSignalrEvents();
    this.gameInitializerService.terrainLoaded.subscribe(terrain => this.onTerrainLoaded(terrain));
    const game = this.gameInitializerService.initializeGame(terrainDataId, canvas);
    this.game = game;

    window.addEventListener('keypress', (e) => {
      if (e.keyCode === 32) {
          console.log(`x: ${this.game.camera.position.x} y: ${this.game.camera.position.y} z: ${this.game.camera.position.z}`);
          console.log('Chunkcoord X: ' + this.currentChunkCoords.x + '   chunkcoord Z: ' + this.currentChunkCoords.y);
      }
  }, false);
  }

  private onTerrainLoaded(terrain: BABYLON.DynamicTerrain) {
    this.terrain = terrain;
    this.storeTerrainSizes(terrain);
    this.registerCameraSetter(this.game.scene, this.game.camera, terrain);
    this.setInitialLocation();
    this.sendMapLoaded();
    this.startRendering(this.game.engine, this.game.scene);
  }

  // TODO this in chunkmanager?
  private storeTerrainSizes(terrain: BABYLON.DynamicTerrain) {
    this.terrainChunkSizeX = terrain.mapSubX;
    this.terrainChunkSizeZ = terrain.mapSubZ;
    this.terrainChunkHalfSizeX = terrain.mapSubX / 2;
    this.terrainChunkHalfSizeZ = terrain.mapSubZ / 2;
  }

  private subscribeToSignalrEvents() {
    this.signalrService.playerIdReceived.subscribe(playerId => this.onPlayerIdReceived(playerId));
    this.signalrService.gameStarted.subscribe(locationDtos => this.onGameStarted(locationDtos));
    this.signalrService.locationsChanged.subscribe(locationDtos => this.onLocationsChanged(locationDtos));
  }

  // TODO this in playermanager
  private onPlayerIdReceived(playerId: number) {
      this.ownLocation.playerId = playerId;

      if (this.players) {
        const ownPlayer = this.players.find(l => l.Location.playerId === this.ownLocation.playerId);
        if (ownPlayer) {
          ownPlayer.Mesh.dispose();
          this.players = this.players.filter(l => l.Location.playerId !== this.ownLocation.playerId);
        }
      }
  }

  private onGameStarted(locationDtos: LocationDto[]) {
    const otherLocations = locationDtos.filter(l => l.playerId !== this.ownLocation.playerId);

    this.players = this.gameInitializerService.initializePlayerModels(otherLocations);
  }

  // TODO this in playermanager
  private onLocationsChanged(locationDtos: LocationDto[]) {
    for (const loc of locationDtos) {
      const player = this.players.find(p => p.Location.playerId === loc.playerId);

      if (player) {
        player.Mesh.position.x = loc.positionX;
        player.Mesh.position.y = loc.positionY;
        player.Mesh.position.z = loc.positionZ;
      }
    }
  }

  private registerCameraSetter(scene: BABYLON.Scene, camera: BABYLON.Camera, terrain: BABYLON.DynamicTerrain): void {
    scene.registerBeforeRender(() => this.setCameraOnGround(camera, terrain));
  }

  private setCameraOnGround(camera: BABYLON.Camera, terrain: BABYLON.DynamicTerrain): void {
    const camAltitude = terrain.getHeightFromMap(camera.position.x, camera.position.z) + this.cameraAltitudeOffset;
    camera.position.y = camAltitude;

    // TODO put this in renderloop \ and a non infinite camera bound
    this.manageChunks(camera);
  }

  // TODO some chunk manager service
  private manageChunks(camera: BABYLON.Camera) {
    // refresh current coords
    this.currentChunkCoords.x = Math.floor((camera.position.x + this.terrainChunkHalfSizeX) / this.terrainChunkSizeX);
    this.currentChunkCoords.y = Math.floor((camera.position.z + this.terrainChunkHalfSizeZ) / this.terrainChunkSizeZ);

    // Check position to request new chunks
    const chunksNeeded: BABYLON.Vector2[] = [];

    // X direction
    const positiveXBoundary =
      this.currentChunkCoords.x * this.terrainChunkSizeX + this.terrainChunkHalfSizeX - this.terrainChunkRequestThreshold;

    if (camera.position.x > positiveXBoundary) {
      chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x + 1, this.currentChunkCoords.y));
    }

    const negativeXBoundary =
      this.currentChunkCoords.x * this.terrainChunkSizeX - this.terrainChunkHalfSizeX + this.terrainChunkRequestThreshold;

    if (camera.position.x < negativeXBoundary) {
      chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x - 1, this.currentChunkCoords.y));
    }

    // Z direction
    const positiveZBoundary =
      this.currentChunkCoords.y * this.terrainChunkSizeZ + this.terrainChunkHalfSizeZ - this.terrainChunkRequestThreshold;

    if (camera.position.z > positiveZBoundary) {
      if (chunksNeeded.length > 0) {
        const xDirection = chunksNeeded[0];
        chunksNeeded.push(new BABYLON.Vector2(xDirection.x, this.currentChunkCoords.y + 1));
      }
      chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x, this.currentChunkCoords.y + 1));
    }

    const negativeZBoundary =
     this.currentChunkCoords.y * this.terrainChunkSizeZ - this.terrainChunkHalfSizeZ + this.terrainChunkRequestThreshold;

    if (camera.position.z < negativeZBoundary) {
      if (chunksNeeded.length > 0) {
        const xDirection = chunksNeeded[0];
        chunksNeeded.push(new BABYLON.Vector2(xDirection.x, this.currentChunkCoords.y - 1));
      }
      chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x, this.currentChunkCoords.y - 1));
    }

    for (const chunkNeeded of chunksNeeded) {
      this.requestChunk(chunkNeeded);
    }
  }

  private requestChunk(coords: BABYLON.Vector2) {
    const requestedChunk = this.requestedChunks.find(ch => ch.x === coords.x && ch.y === coords.y);

    if (!requestedChunk) {
      // TODO request chunk from server
      this.requestedChunks.push(coords);
    }

    // else, chunk has already been requested, nothing to do
  }

  // TODO this in init service?
  private setInitialLocation() {
    this.game.camera.position.x = this.getRandom(-this.terrain.mapSubX / 2, this.terrain.mapSubX / 2);
    this.game.camera.position.z = this.getRandom(-this.terrain.mapSubZ / 2, this.terrain.mapSubZ / 2);
    this.setCameraOnGround(this.game.camera, this.terrain);

    this.ownLocation = new LocationDto({
      playerId: 0,
      positionX: this.game.camera.position.x,
      positionY: this.game.camera.position.y,
      positionZ: this.game.camera.position.z,
      rotationX: 0,
      rotationY: 0,
      rotationZ: 0
    });
  }

  private sendMapLoaded() {
    this.signalrService.sendMapLoaded(new MapLoadedMessage({
      sessionId: this.sessionId,
      location: this.ownLocation
    }));
  }

  private startRendering(engine: BABYLON.Engine, scene: BABYLON.Scene): void {
    engine.runRenderLoop(() => {

      // TODO main loop, instruct playermanager to do stuff:
      this.updateOwnLocation();

      this.signalrService.sendLocationChanged(new LocationChangedMessage({
        sessionId: this.sessionId,
        location: this.ownLocation
      }));

      // TODO instruct chunkmanager to do stuff:

      scene.render();
    });
  }

  // TODO into player manager
  private updateOwnLocation() {
    this.ownLocation.positionX = this.game.camera.position.x;
    this.ownLocation.positionY = this.game.camera.position.y - (this.cameraAltitudeOffset / 2);
    this.ownLocation.positionZ = this.game.camera.position.z;
  }

  private getRandom(min: number, max: number) {
    return Math.random() * (max - min) + min;
  }
}
