/// <reference types="babylonjs"/>
// tslint:disable-next-line:no-reference
/// <reference path="../extensions/babylon.dynamicTerrain.d.ts"/>

import '../extensions/babylon.dynamicTerrain.js';

import { Injectable } from '@angular/core';
import { SignalRService } from 'src/app/shared/signalr.service';
import { Player } from '../interfaces/player.js';
import { LocationDto, LocationChangedMessage, MapLoadedMessage } from 'src/app/_models/generatedDtos.js';
import { Game } from '../interfaces/game.js';
import { TerrainData } from '../interfaces/terrainData.js';
import { GameInitializerService } from './game-initializer.service.js';

@Injectable({
  providedIn: 'root'
})
export class PlayerManagerService {

  private players: Player[];
  private ownLocation: LocationDto;
  private cameraAltitudeOffset = 3;
  private game: Game;
  private sessionId: string;

  constructor(private signalrService: SignalRService, private gameInitializerService: GameInitializerService) { }

  public initialize(game: Game, terrainData: TerrainData, sessionId: string) {
    this.game = game;
    this.sessionId = sessionId;
    this.subscribeToSignalrEvents();
    this.setInitialLocation(terrainData);
    this.registerCameraSetter(this.game.scene, this.game.camera, terrainData.terrain);
    this.sendMapLoaded();
  }

  private subscribeToSignalrEvents() {
    this.signalrService.playerIdReceived.subscribe(playerId => this.onPlayerIdReceived(playerId));
    this.signalrService.gameStarted.subscribe(locationDtos => this.onGameStarted(locationDtos));
    this.signalrService.locationsChanged.subscribe(locationDtos => this.onLocationsChanged(locationDtos));
  }

  private setInitialLocation(terrainData: TerrainData) {
    this.game.camera.position.x = this.getRandom(-terrainData.heightmapInfo.width / 2, terrainData.heightmapInfo.width / 2);
    this.game.camera.position.z = this.getRandom(-terrainData.heightmapInfo.height / 2, terrainData.heightmapInfo.height / 2);
    this.setCameraOnGround(this.game.camera, terrainData.terrain);

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

  private registerCameraSetter(scene: BABYLON.Scene, camera: BABYLON.Camera, terrain: BABYLON.DynamicTerrain): void {
    scene.registerBeforeRender(() => this.setCameraOnGround(camera, terrain));
  }

  private setCameraOnGround(camera: BABYLON.Camera, terrain: BABYLON.DynamicTerrain): void {
    const camAltitude = terrain.getHeightFromMap(camera.position.x, camera.position.z) + this.cameraAltitudeOffset;
    camera.position.y = camAltitude;
  }

  private sendMapLoaded() {
    this.signalrService.sendMapLoaded(new MapLoadedMessage({
      sessionId: this.sessionId,
      location: this.ownLocation
    }));
  }

  private onGameStarted(locationDtos: LocationDto[]) {
    const otherLocations = locationDtos.filter(l => l.playerId !== this.ownLocation.playerId);

    this.players = this.gameInitializerService.initializePlayerModels(otherLocations);
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

  private onLocationsChanged(locationDtos: LocationDto[]) {
    for (const loc of locationDtos) {
      const player = this.players.find(p => p.Location.playerId === loc.playerId);

      if (player) {
        player.Mesh.position.x = loc.positionX;
        player.Mesh.position.y = loc.positionY;
        player.Mesh.position.z = loc.positionZ;
        player.Mesh.rotation.y = loc.rotationY;
      }
    }
  }

  public update() {
    this.ownLocation.positionX = this.game.camera.position.x;
    this.ownLocation.positionY = this.game.camera.position.y - (this.cameraAltitudeOffset / 2);
    this.ownLocation.positionZ = this.game.camera.position.z;
    this.ownLocation.rotationY = this.game.camera.rotation.y;

    this.signalrService.sendLocationChanged(new LocationChangedMessage({
      sessionId: this.sessionId,
      location: this.ownLocation
    }));
  }

  private getRandom(min: number, max: number) {
    return Math.random() * (max - min) + min;
  }
}
