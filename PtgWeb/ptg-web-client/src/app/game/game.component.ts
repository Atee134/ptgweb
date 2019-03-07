
/// <reference types="babylonjs"/>
// tslint:disable-next-line:no-reference
/// <reference path="./extensions/babylon.dynamicTerrain.d.ts"/>

import './extensions/babylon.dynamicTerrain.js';

import { Component, OnInit, ViewChild } from '@angular/core';
import { HeightmapService } from '../_services/heightmap.service';
import { DiamondSquareHeightmapRequestDto } from '../_models/generatedDtos';
import { ActivatedRoute } from '@angular/router';
import { TerrainMaterial } from 'babylonjs-materials';


@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  @ViewChild('viewport') viewPort: any;
  private canvas: HTMLCanvasElement;
  private engine: BABYLON.Engine;
  private scene: BABYLON.Scene;
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

  private resolution = { width: 1920, height: 1080 }; // TODO add this as an input component, and make it selectable from the settings
  private terrainDataId: string;

  constructor(private route: ActivatedRoute, private heightmapService: HeightmapService) { }

  ngOnInit() {
    this.terrainDataId = this.route.snapshot.paramMap.get('terrainDataId');
    this.canvas = this.initializeCanvas(this.resolution);
    this.engine = this.initializeEngine(this.canvas);
    this.scene = this.initializeScene(this.engine, this.canvas);
    this.initDynamicTerrain(this.scene, this.terrainDataId);
    this.startRendering();
  }

  private initializeCanvas(resolution: any): HTMLCanvasElement {
    const canvas = this.viewPort.nativeElement as HTMLCanvasElement;
    canvas.style.width = `${resolution.width}px`;
    canvas.style.height = `${resolution.height}px`;

    return canvas;
  }

  private initializeEngine(canvas: HTMLCanvasElement): BABYLON.Engine {
    const engine = new BABYLON.Engine(canvas, true);
    return engine;
  }

  private initializeScene(engine: BABYLON.Engine, canvas: HTMLCanvasElement): BABYLON.Scene {
    const scene = new BABYLON.Scene(engine);
    scene.gravity = this.sceneSettings.gravity;
    scene.collisionsEnabled = true;
    scene.clearColor = this.sceneSettings.color;
    const light = new BABYLON.DirectionalLight('DirectionalLight', this.sceneSettings.lightDirection, scene);
    light.intensity = this.sceneSettings.lightIntensity;

    this.initializeCamera(scene, this.sceneSettings.cameraStartPosition, this.sceneSettings.cameraSize, canvas);

    return scene;
  }

  private initDynamicTerrain(scene: any, terrainDataId: string) {
    this.heightmapService.getHeightmap(terrainDataId).subscribe(heightmapDto => {
      const terrainMaterial = new TerrainMaterial('terrainMaterial', scene);
      terrainMaterial.diffuseColor = BABYLON.Color3.Green();
      terrainMaterial.wireframe = true;

      const params = {
          mapData : heightmapDto.heightmapCoords,
          mapSubX : heightmapDto.width,
          mapSubZ : heightmapDto.height,
          terrainSub : 100
      };
      const terrain = new BABYLON.DynamicTerrain('terrain', params, scene);
      terrain.mesh.material = terrainMaterial;
      terrain.subToleranceX = 8;
      terrain.subToleranceZ = 8;
      terrain.LODLimits = [4, 3, 2, 1, 1];
    });
  }

  private initializeCamera(scene: BABYLON.Scene, startPosition: BABYLON.Vector3, cameraSize: BABYLON.Vector3, canvas: HTMLCanvasElement) {
    const camera = new BABYLON.UniversalCamera('universalCamera', startPosition, scene);
    camera.attachControl(canvas);
    camera.ellipsoid = cameraSize;
    camera.checkCollisions = true;
    camera.applyGravity = true;

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

  private startRendering() {
    this.engine.runRenderLoop(() => {
      this.scene.render();
    });
  }
}
