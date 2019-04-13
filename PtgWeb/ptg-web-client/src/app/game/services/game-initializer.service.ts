/// <reference types="babylonjs"/>
// tslint:disable-next-line:no-reference
/// <reference path="../extensions/babylon.dynamicTerrain.d.ts"/>

import '../extensions/babylon.dynamicTerrain.js';

import { Injectable } from '@angular/core';
import { TerrainMaterial } from 'babylonjs-materials';
import { HeightmapService } from '../../_services/heightmap.service';
import { Subject } from 'rxjs';
import { LocationDto, HeightmapInfoResponseDto } from '../../_models/generatedDtos.js';
import { Player } from '../interfaces/player.js';
import { Game } from '../interfaces/game.js';
import { TerrainData } from '../interfaces/terrainData.js';

@Injectable({
  providedIn: 'root'
})
export class GameInitializerService {

  public terrainLoaded = new Subject<TerrainData>();
  private heightmapInfo: HeightmapInfoResponseDto;

  private scene: BABYLON.Scene;
  private engine: BABYLON.Engine;
  private terrainDataId: string;
  private sceneSettings = {
    gravity: new BABYLON.Vector3(0, -0.2, 0),
    color: new BABYLON.Color4(0.275, 0.82, 1, 1),
    lightDirection: new BABYLON.Vector3(0, -1, 0),
    lightIntensity: 0.5,
    cameraStartPosition: new BABYLON.Vector3(0, 50, -10),
    cameraSize: new BABYLON.Vector3(1, 1, 1)
  };

  private cameraControls = {
    keysUp: ['W'.charCodeAt(0), 'w'.charCodeAt(0), 38],
    keysDown: ['S'.charCodeAt(0), 's'.charCodeAt(0), 40],
    keysRight: ['D'.charCodeAt(0), 'd'.charCodeAt(0), 39],
    keysLeft: ['A'.charCodeAt(0), 'a'.charCodeAt(0), 37]
  };

  private resolution = { width: 1920, height: 1080 };

  private alreadyLocked = false;

  constructor(private heightmapService: HeightmapService) { }

  public initializeGame(terrainDataId: string, canvas: HTMLCanvasElement): Game {
    this.terrainDataId = terrainDataId;
    this.engine = this.initializeEngine(canvas);
    this.setCanvasResolution(canvas);
    const scene = this.initializeScene(this.engine);
    this.scene = scene;
    const camera = this.initializeCamera(scene, this.sceneSettings.cameraStartPosition, this.sceneSettings.cameraSize, canvas);
    this.createSkyBox(scene);
    this.initDynamicTerrain(scene, terrainDataId);
    this.addResizeListener(canvas);
    this.addPointerLockListeners(canvas);

    const game: Game = {
      engine: this.engine,
      scene,
      camera,
    };

    return game;
  }

  public initializePlayerModels(locations: LocationDto[]): Player[] {
    const players: Player[] = [];
    for (const loc of locations) {
      const head = BABYLON.MeshBuilder.CreateSphere('head', {diameter: 0.55}, this.scene);
      head.position.y = 1.5;
      const body = BABYLON.MeshBuilder.CreateSphere('body', {diameterX: 1, diameterY: 3, diameterZ: 0.6}, this.scene);
      const backpack = BABYLON.MeshBuilder.CreateBox('backpack', {height: 1.15, width: 0.65, depth: 0.25}, this.scene);
      backpack.position.z = -0.4;
      backpack.position.y = 0.25;

      const playerMesh = BABYLON.Mesh.MergeMeshes([head, body, backpack], true);

      playerMesh.position.x = loc.positionX;
      playerMesh.position.y = loc.positionY;
      playerMesh.position.z = loc.positionZ;

      const playerMat = new BABYLON.StandardMaterial('backpackMat', this.scene);
      playerMat.diffuseColor = new BABYLON.Color3(1, 1, 1);
      playerMat.ambientColor = new BABYLON.Color3(0.4, 0.8, 0.4);
      playerMat.emissiveColor = new BABYLON.Color3(0.2, 0.2, 0.2);
      playerMesh.material = playerMat;

      const player: Player = {
        Mesh: playerMesh,
        Location: loc
      };

      players.push(player);
    }

    return players;
  }



  private initializeEngine(canvas: HTMLCanvasElement): BABYLON.Engine {
    const engine = new BABYLON.Engine(canvas, true);
    return engine;
  }

  private initializeScene(engine: BABYLON.Engine): BABYLON.Scene {
    const scene = new BABYLON.Scene(engine);
    scene.gravity = this.sceneSettings.gravity;
    scene.collisionsEnabled = true;
    scene.clearColor = this.sceneSettings.color;
    const light = new BABYLON.DirectionalLight('DirectionalLight', this.sceneSettings.lightDirection, scene);
    light.intensity = this.sceneSettings.lightIntensity;

    return scene;
  }

  private createSkyBox(scene: BABYLON.Scene): void {
    const skybox = BABYLON.MeshBuilder.CreateBox('skyBox', {size: 10000.0}, scene);
    const skyboxMaterial = new BABYLON.StandardMaterial('skyBox', scene);
    skyboxMaterial.backFaceCulling = false;
    skyboxMaterial.reflectionTexture = new BABYLON.CubeTexture('assets/textures/skybox/miramar', scene);
    skyboxMaterial.reflectionTexture.coordinatesMode = BABYLON.Texture.SKYBOX_MODE;
    skyboxMaterial.diffuseColor = new BABYLON.Color3(0, 0, 0);
    skyboxMaterial.specularColor = new BABYLON.Color3(0, 0, 0);
    skybox.material = skyboxMaterial;
}

  private createTerrainMaterial(scene: BABYLON.Scene, splatmapUrl: string, width: number, height: number): TerrainMaterial {
  const terrainMaterial = new TerrainMaterial('terrainMaterial', scene);
  terrainMaterial.specularColor = new BABYLON.Color3(0.5, 0.5, 0.5);
  terrainMaterial.specularPower = 64;

  terrainMaterial.mixTexture = new BABYLON.Texture(splatmapUrl, scene);

  // RED, GREEN, BLUE
  terrainMaterial.diffuseTexture1 = new BABYLON.Texture('assets/textures/ground/grass.jpg', scene);
  terrainMaterial.diffuseTexture2 = new BABYLON.Texture('assets/textures/ground/rock.jpg', scene);
  terrainMaterial.diffuseTexture3 = new BABYLON.Texture('assets/textures/ground/snow.jpg', scene);

  const size = width < height ? width : height;

  terrainMaterial.diffuseTexture1.uScale = terrainMaterial.diffuseTexture1.vScale = size / 10;
  terrainMaterial.diffuseTexture2.uScale = terrainMaterial.diffuseTexture2.vScale = size / 8;
  terrainMaterial.diffuseTexture3.uScale = terrainMaterial.diffuseTexture3.vScale = size / 8;

  return terrainMaterial;
  }

  private initDynamicTerrain(scene: BABYLON.Scene, terrainDataId: string): void {
    this.heightmapService.getHeightmapInfo(terrainDataId).subscribe(heightmapInfo => {
      this.heightmapInfo = heightmapInfo;
      const heightmapUrl = this.heightmapService.getHeightmapUrl(terrainDataId);

      const actualWidth = heightmapInfo.width + heightmapInfo.overlappedSize * 2;
      const actualHeight = heightmapInfo.height + heightmapInfo.overlappedSize * 2;

      const heightmapOptions = {
              width: actualWidth, height: actualHeight,
              subX: actualWidth, subZ: actualHeight,
              onReady: this.createTerrain.bind(this),
              minHeight: 0,
              maxHeight: 20
      };

      const mapData = new Float32Array(actualWidth * actualHeight * 3);
      BABYLON.DynamicTerrain.CreateMapFromHeightMapToRef(heightmapUrl, heightmapOptions as any, mapData, scene);
    });
  }

  private createTerrain(mapData: number[], mapSubX: number, mapSubZ: number): void {
    const options = {
      terrainSub: 200,
      mapData,
      mapSubX,
      mapSubZ
    };
    const terrainMaterial = this.createTerrainMaterial(
      this.scene,
      this.heightmapService.getSplatmapUrl(this.terrainDataId),
      mapSubX,
      mapSubZ);

    const terrain = new BABYLON.DynamicTerrain('terrain', options, this.scene);
    terrain.mesh.material = terrainMaterial;
    terrain.subToleranceX = 16;
    terrain.subToleranceZ = 16;
    terrain.LODLimits = [32, 16, 8, 4, 2, 1];
    terrain.createUVMap();
    terrain.update(true);

    const terrainData: TerrainData = {
      terrain,
      heightmapInfo: this.heightmapInfo
    };

    this.terrainLoaded.next(terrainData);
  }

  private initializeCamera(scene: BABYLON.Scene,
                           startPosition: BABYLON.Vector3,
                           cameraSize: BABYLON.Vector3,
                           canvas: HTMLCanvasElement): BABYLON.UniversalCamera {
    const camera = new BABYLON.UniversalCamera('universalCamera', startPosition, scene);
    camera.attachControl(canvas, true);
    camera.ellipsoid = cameraSize;
    camera.setTarget(BABYLON.Vector3.Zero());

    for (const charCode of this.cameraControls.keysUp) {
      camera.keysUp.push(charCode);
    }

    for (const charCode of this.cameraControls.keysDown) {
      camera.keysDown.push(charCode);
    }

    for (const charCode of this.cameraControls.keysLeft) {
      camera.keysLeft.push(charCode);
    }

    for (const charCode of this.cameraControls.keysRight) {
      camera.keysRight.push(charCode);
    }

    return camera;
  }

  private addResizeListener(canvas: HTMLCanvasElement) {
    window.addEventListener('resize', () => {
      this.setCanvasResolution(canvas);
    });
  }

  private setCanvasResolution(canvas: HTMLCanvasElement): void {
    canvas.style.width = `${this.resolution.width}px`;
    canvas.style.height = `${this.resolution.height}px`;
    this.engine.resize();
    canvas.style.width = '100%';
    canvas.style.height = '100%';
  }

  private addPointerLockListeners(canvas: HTMLCanvasElement) {
      this.scene.onPointerDown = () => {
          if (!this.alreadyLocked) {
              canvas.requestPointerLock = canvas.requestPointerLock
              || canvas.msRequestPointerLock
              || canvas.mozRequestPointerLock
              || canvas.webkitRequestPointerLock;
              canvas.requestPointerLock();
          }
      };

      document.addEventListener('pointerlockchange', this.pointerLockListener);
      document.addEventListener('mspointerlockchange', this.pointerLockListener);
      document.addEventListener('mozpointerlockchange', this.pointerLockListener);
      document.addEventListener('webkitpointerlockchange', this.pointerLockListener);
  }

  private pointerLockListener() {
    const doc = document as any;
    const element = doc.pointerLockElement
    || doc.msPointerLockElement
    || doc.mozPointerLockElement
    || doc.webkitPointerLockElement || null;

    if (element) {
      this.alreadyLocked = true;
    } else {
      this.alreadyLocked = false;
    }
  }
}
