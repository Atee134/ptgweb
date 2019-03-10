/// <reference types="babylonjs"/>
// tslint:disable-next-line:no-reference
/// <reference path="./extensions/babylon.dynamicTerrain.d.ts"/>

import './extensions/babylon.dynamicTerrain.js';

import { Injectable } from '@angular/core';
import { GameInitializerService } from './game-initializer.service';
import { SignalRService } from 'src/app/shared/signalr.service';
import { MapLoadedMessage, LocationDto } from '../_models/generatedDtos.js';

@Injectable({
  providedIn: 'root'
})
export class GameManagerService {
  private ownPlayerId: number;
  private sessionId: string;
  private game: Game;

  constructor(private gameInitializerService: GameInitializerService, private signalrService: SignalRService) { }

  public startGame(sessionId: string, terrainDataId: string, canvas: HTMLCanvasElement) {
    this.sessionId = sessionId;
    const game = this.gameInitializerService.initializeGame(terrainDataId, canvas);
    this.game = game;
    this.subscribeToSignalrEvents();
    this.startRendering(this.game.engine, this.game.scene);
    this.sendMapLoaded();
  }

  private subscribeToSignalrEvents() {
    this.signalrService.playerIdReceived.subscribe(playerId => {
      this.ownPlayerId = playerId;
    });
  }

  private startRendering(engine: BABYLON.Engine, scene: BABYLON.Scene): void {
    engine.runRenderLoop(() => {
      scene.render();
    });
  }

  private sendMapLoaded() {
    this.signalrService.sendMapLoaded(new MapLoadedMessage({
      sessionId: this.sessionId,
      location: new LocationDto({
        playerId: 0,
        positionX: 5,
        positionY: 5,
        positionZ: 5,
        rotationX: 5,
        rotationY: 5,
        rotationZ: 5
      })
    }));
  }
}
