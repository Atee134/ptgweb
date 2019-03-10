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
  private terrain: BABYLON.DynamicTerrain;
  private cameraAltitudeOffset = 3;

  constructor(private gameInitializerService: GameInitializerService, private signalrService: SignalRService) { }

  public startGame(sessionId: string, terrainDataId: string, canvas: HTMLCanvasElement) {
    this.sessionId = sessionId;
    this.gameInitializerService.terrainLoaded.subscribe(terrain => this.onTerrainLoaded(terrain));
    const game = this.gameInitializerService.initializeGame(terrainDataId, canvas);
    this.game = game;
    this.subscribeToSignalrEvents();

    window.addEventListener('keypress', (e) => {
      console.log('keypress event raised');
      if (e.keyCode === 32) {
          console.log(`x: ${this.game.camera.position.x} y: ${this.game.camera.position.y} z: ${this.game.camera.position.z}`);
      }
  }, false);
  }

  private onTerrainLoaded(terrain: BABYLON.DynamicTerrain) {
    this.terrain = terrain;
    this.registerCameraSetter(this.game.scene, this.game.camera, terrain);
    this.setInitialLocation();
    this.sendMapLoaded();
    this.startRendering(this.game.engine, this.game.scene);
  }

  private subscribeToSignalrEvents() {
    this.signalrService.playerIdReceived.subscribe(playerId => this.onPlayerIdReceived(playerId));
    this.signalrService.gameStarted.subscribe(locationDtos => this.onGameStarted(locationDtos));
    this.signalrService.locationsChanged.subscribe(locationDtos => this.onLocationsChanged(locationDtos));
  }

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
  }

  private setInitialLocation() {
    this.game.camera.position.x = this.getRandom(-this.terrain.terrainHalfSizeX, this.terrain.terrainHalfSizeX);
    this.game.camera.position.z = this.getRandom(-this.terrain.terrainHalfSizeZ, this.terrain.terrainHalfSizeZ);
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
      this.updateOwnLocation();

      this.signalrService.sendLocationChanged(new LocationChangedMessage({
        sessionId: this.sessionId,
        location: this.ownLocation
      }));

      scene.render();
    });
  }

  private updateOwnLocation() {
    this.ownLocation.positionX = this.game.camera.position.x;
    this.ownLocation.positionY = this.game.camera.position.y;
    this.ownLocation.positionZ = this.game.camera.position.z;
  }

  private getRandom(min: number, max: number) {
    return Math.random() * (max - min) + min;
  }
}
