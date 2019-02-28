import { Component, OnInit, ViewChild } from '@angular/core';
import { Scene, Engine, Vector3, Color4, DirectionalLight, UniversalCamera } from 'babylonjs';
import { HeightmapService } from '../_services/heightmap.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  @ViewChild('viewport') viewPort: any;
  private canvas: HTMLCanvasElement;
  private engine: Engine;
  private scene: Scene;
  private sceneSettings = {
    gravity: new Vector3(0, -0.2, 0),
    color: new Color4(0.275, 0.82, 1, 1),
    lightDirection: new Vector3(0, -1, 0),
    lightIntensity: 0.5,
    cameraStartPosition: new Vector3(0, 50, -10),
    cameraSize: new Vector3(1, 1, 1)
  };

  private cameraControls = {
    keysUp: ['W'.charCodeAt(0), 'w'.charCodeAt(0), 38],
    keysDown: ['S'.charCodeAt(0), 's'.charCodeAt(0), 40],
    keysRight: ['D'.charCodeAt(0), 'd'.charCodeAt(0), 39],
    keysLeft: ['A'.charCodeAt(0), 'a'.charCodeAt(0), 37]
  };

  private resolution = { width: 1920, height: 1080 }; // TODO add this as an input component, and make it selectable from the settings
  private terrainDataId: string;

  constructor(private heightmapService: HeightmapService) { }

  ngOnInit() {
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

  private initializeEngine(canvas: HTMLCanvasElement): Engine {
    const engine = new Engine(canvas, true);
    return engine;
  }

  private initializeScene(engine: Engine, canvas: HTMLCanvasElement): Scene {
    const scene = new Scene(engine);
    scene.gravity = this.sceneSettings.gravity;
    scene.collisionsEnabled = true;
    scene.clearColor = this.sceneSettings.color;
    const light = new DirectionalLight('DirectionalLight', this.sceneSettings.lightDirection, scene);
    light.intensity = this.sceneSettings.lightIntensity;

    this.initializeCamera(scene, this.sceneSettings.cameraStartPosition, this.sceneSettings.cameraSize, canvas);

    return scene;
  }

  private initializeCamera(scene: Scene, startPosition: Vector3, cameraSize: Vector3, canvas: HTMLCanvasElement) {
    const camera = new UniversalCamera('universalCamera', startPosition, scene);
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
    this.heightmapService.generateDiamondSquareHeightmap(513, 1500, 0.4).subscribe(resp => {
      this.terrainDataId = resp;
      this.heightmapService.createGround(this.terrainDataId, null, this.scene);
    });
  }

  private startRendering() {
    this.engine.runRenderLoop(() => {
      this.scene.render();
    });
  }
}
