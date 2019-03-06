
/// <reference types="babylonjs"/>
// tslint:disable-next-line:no-reference
/// <reference path="./extensions/babylon.dynamicTerrain.d.ts"/>

import './extensions/babylon.dynamicTerrain.js';

import { Component, OnInit, ViewChild } from '@angular/core';
import { HeightmapService } from '../_services/heightmap.service';
import { DiamondSquareHeightmapRequestDto } from '../_models/generatedDtos';
import { ActivatedRoute } from '@angular/router';


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
    this.buildTerrain();
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


    // TODO Dynamic
    this.initDynamicTerrain(scene);

    return scene;
  }

  private initDynamicTerrain(scene: any) {
    var mapSubX = 1000;             // point number on X axis
var mapSubZ = 800;              // point number on Z axis
var elevationScale = 6.0;
var mapData = new Float32Array(mapSubX * mapSubZ * 3); // 3 float values per point : x, y and z

var paths = [];                             // array for the ribbon model
for (var l = 0; l < mapSubZ; l++) {
    var path = [];                          // only for the ribbon
    for (var w = 0; w < mapSubX; w++) {
        var x = (w - mapSubX * 0.5) * 2.0;
        var z = (l - mapSubZ * 0.5) * 2.0;
        var y = Math.random();
        y *= (0.5 + y) * (Math.random()*5) * elevationScale;   // let's increase a bit the noise computed altitude
                
        mapData[3 *(l * mapSubX + w)] = x;
        mapData[3 * (l * mapSubX + w) + 1] = y;
        mapData[3 * (l * mapSubX + w) + 2] = z;
        
        path.push(new BABYLON.Vector3(x, y, z));
    }
    paths.push(path);
}

var map = BABYLON.MeshBuilder.CreateRibbon("m", {pathArray: paths, sideOrientation: 2}, scene);
map.position.y = -1.0;
var mapMaterial = new BABYLON.StandardMaterial("mm", scene);
mapMaterial.wireframe = true;
mapMaterial.alpha = 0.5;
map.material = mapMaterial;

// wait for dynamic terrain extension to be loaded
// Dynamic Terrain
// ===============
var terrainSub = 100;               // 100 terrain subdivisions
var params = {
    mapData: mapData,               // data map declaration : what data to use ?
    mapSubX: mapSubX,               // how are these data stored by rows and columns
    mapSubZ: mapSubZ,
    terrainSub: terrainSub          // how many terrain subdivisions wanted
}
var terrain = new BABYLON.DynamicTerrain("t", params, <any>scene);
var terrainMaterial = new BABYLON.StandardMaterial("tm", scene);
terrainMaterial.diffuseColor = BABYLON.Color3.Green();
//terrainMaterial.alpha = 0.8;
terrainMaterial.wireframe = true;
terrain.mesh.material = <any>terrainMaterial;
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

  private buildTerrain() {
   // this.heightmapService.createGround(this.terrainDataId, null, this.scene);
  }

  private startRendering() {
    this.engine.runRenderLoop(() => {
      this.scene.render();
    });
  }
}
