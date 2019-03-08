
/// <reference types="babylonjs"/>
// tslint:disable-next-line:no-reference
/// <reference path="./extensions/babylon.dynamicTerrain.d.ts"/>

import './extensions/babylon.dynamicTerrain.js';

import { Component, OnInit, ViewChild } from '@angular/core';
import { HeightmapService } from '../_services/heightmap.service';
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
  private camera: BABYLON.Camera;
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
    this.scene = this.initializeScene(this.engine);
    this.camera = this.initializeCamera(this.scene, this.sceneSettings.cameraStartPosition, this.sceneSettings.cameraSize, this.canvas);
    this.createSkyBox(this.scene);
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

  private createTerrainMaterial(scene: BABYLON.Scene, splatmapUrl: string): TerrainMaterial {
    // Create terrain material
  const terrainMaterial = new TerrainMaterial('terrainMaterial', scene);
  terrainMaterial.specularColor = new BABYLON.Color3(0.5, 0.5, 0.5);
  terrainMaterial.specularPower = 64;

  terrainMaterial.mixTexture = new BABYLON.Texture(splatmapUrl, scene);

  // RED, GREEN, BLUE
  terrainMaterial.diffuseTexture1 = new BABYLON.Texture('assets/textures/ground/grass.jpg', scene);
  terrainMaterial.diffuseTexture2 = new BABYLON.Texture('assets/textures/ground/rock.jpg', scene);
  terrainMaterial.diffuseTexture3 = new BABYLON.Texture('assets/textures/ground/snow.jpg', scene);

  terrainMaterial.diffuseTexture1.uScale = terrainMaterial.diffuseTexture1.vScale = 30;
  terrainMaterial.diffuseTexture2.uScale = terrainMaterial.diffuseTexture2.vScale = 20;
  terrainMaterial.diffuseTexture3.uScale = terrainMaterial.diffuseTexture3.vScale = 20;

  return terrainMaterial;
  }

  private initDynamicTerrain(scene: any, terrainDataId: string) {
      this.heightmapService.getHeightmapInfo(terrainDataId).subscribe(heightmapInfo => {
        const heightmapUrl = this.heightmapService.getHeightmapUrl(terrainDataId);
        const heightmapOptions = {
                width: heightmapInfo.width, height: heightmapInfo.height,
                subX: heightmapInfo.width, subZ: heightmapInfo.height,
                onReady: this.createTerrain.bind(this),
                minHeight: 0,
                maxHeight: 20
        };
        const mapData = new Float32Array(heightmapInfo.width * heightmapInfo.height * 3);
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
    const terrainMaterial = this.createTerrainMaterial(this.scene, this.heightmapService.getSplatmapUrl(this.terrainDataId));
    const terrain = new BABYLON.DynamicTerrain('terrain', options, this.scene);
    terrain.mesh.material = terrainMaterial;
    terrain.subToleranceX = 16;
    terrain.subToleranceZ = 16;
    terrain.LODLimits = [4, 3, 2, 1, 1];
    terrain.createUVMap();

    this.scene.registerBeforeRender(() => this.setCameraHeight(this.camera, terrain));
  }

  private initializeCamera(scene: BABYLON.Scene,
                           startPosition: BABYLON.Vector3,
                           cameraSize: BABYLON.Vector3,
                           canvas: HTMLCanvasElement): BABYLON.Camera {
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

  private setCameraHeight(camera: BABYLON.Camera, terrain: BABYLON.DynamicTerrain) {
    const camAltitude = terrain.getHeightFromMap(camera.position.x, camera.position.z) + 3;
    camera.position.y = camAltitude;
  }

  private startRendering() {
    this.engine.runRenderLoop(() => {
      this.scene.render();
    });
  }
}
