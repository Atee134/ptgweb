/// <reference types="babylonjs"/>
// tslint:disable-next-line:no-reference
/// <reference path="../extensions/babylon.dynamicTerrain.d.ts"/>

import '../extensions/babylon.dynamicTerrain.js';

import { Injectable } from '@angular/core';
import { GameInitializerService } from './game-initializer.service';
import { SignalRService } from 'src/app/shared/signalr.service';
import { MapLoadedMessage, LocationDto, LocationChangedMessage, HeightmapInfoResponseDto } from '../../_models/generatedDtos.js';
import { Game } from '../interfaces/game.js';
import { Player } from '../interfaces/player.js';
import { ChunkManagerService } from './chunk-manager.service';
import { TerrainData } from '../interfaces/terrainData.js';
import { PlayerManagerService } from './player-manager.service.js';

@Injectable({
  providedIn: 'root'
})
export class GameManagerService {
  private sessionId: string;
  private game: Game;
  private terrainDataId: string;
  private infinite: boolean;

  constructor(
    private gameInitializerService: GameInitializerService,
    private signalrService: SignalRService,
    private chunkManagerService: ChunkManagerService,
    private playerManagerService: PlayerManagerService) { }

  public startGame(sessionId: string, terrainDataId: string, canvas: HTMLCanvasElement, infinite: boolean) {
    this.sessionId = sessionId;
    this.terrainDataId = terrainDataId;
    this.infinite = infinite;
    this.gameInitializerService.terrainLoaded.subscribe(terrainData => this.onTerrainLoaded(terrainData));
    const game = this.gameInitializerService.initializeGame(terrainDataId, canvas);
    this.game = game;


    window.addEventListener('keypress', (e) => {
      if (e.keyCode === 32) {
          console.log(`x: ${this.game.camera.position.x} y: ${this.game.camera.position.y} z: ${this.game.camera.position.z}`);
      }
  }, false);
  }

  private onTerrainLoaded(terrainData: TerrainData) {
    this.chunkManagerService.initialize(terrainData, this.terrainDataId, this.game.scene);
    this.playerManagerService.initialize(this.game, terrainData, this.sessionId);
    this.startRendering(this.game.engine, this.game.scene);
  }

  private startRendering(engine: BABYLON.Engine, scene: BABYLON.Scene): void {
    engine.runRenderLoop(() => {

      this.playerManagerService.update();

      if (this.infinite) {
        this.chunkManagerService.manageChunks(this.game.camera.position);
      } else {
        // TODO add invisible camera wall here
      }

      scene.render();
    });
  }

}
