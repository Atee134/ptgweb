import { Scene, Engine, Vector3, UniversalCamera, Color3, Mesh } from 'babylonjs';
import * as signalR from "@aspnet/signalr";

const baseApiUrl = 'http://localhost:5000/api/';

document.addEventListener('DOMContentLoaded', startGame);

const resolution = { width: 1920, height: 1080 };
let scene: Scene;
let engine: Engine;
let canvas: HTMLCanvasElement;
let camera: UniversalCamera;

window.addEventListener('keypress', function (e) {
    this.console.log('keypress event raised');
    if (e.keyCode === 32) {
        signalRTester();
    }
}, false);

function signalRTester() {
    sendPosition(camera.position);
}

function startGame() {
    canvas = <HTMLCanvasElement> document.getElementById('renderCanvas');

    canvas.style.width = `${resolution.width}px`;
    canvas.style.height = `${resolution.height}px`;
    engine = new Engine(canvas, true);
    canvas.style.width = "100%";
    canvas.style.height = "100%";

    scene = createScene();

    var ground = Mesh.CreateGroundFromHeightMap('myGround', baseApiUrl + 'heightmap/fault?width=256&height=256&iterationCount=200&offsetPerIteration=5', 256, 256, 150, 0, 15, scene);
    ground.checkCollisions = true;

    engine.runRenderLoop(renderLoop);
}

function createScene(): Scene {
    scene = new Scene(engine);
    scene.gravity = new Vector3(0, -0.2, 0);
    scene.collisionsEnabled = true;

    camera = createFreeCamera(scene);
    var light = new BABYLON.DirectionalLight("DirectionalLight", new BABYLON.Vector3(0, -1, 0), scene);
    light.intensity = 0.5;
    return scene;
}

function createFreeCamera(scene) {
    let cam = new UniversalCamera('universalCamera', new BABYLON.Vector3(0, 5, -10), scene);
    cam.attachControl(canvas);
    cam.ellipsoid = new BABYLON.Vector3(1, 1, 1);
    cam.checkCollisions = true;
    cam.applyGravity = true;

    cam.keysUp.push('w'.charCodeAt(0));
    cam.keysUp.push('W'.charCodeAt(0));

    cam.keysDown.push('s'.charCodeAt(0));
    cam.keysDown.push('S'.charCodeAt(0));
    
    cam.keysLeft.push('a'.charCodeAt(0));
    cam.keysLeft.push('A'.charCodeAt(0));

    cam.keysRight.push('d'.charCodeAt(0));
    cam.keysRight.push('D'.charCodeAt(0));

    return cam;
}

function renderLoop() {
    scene.render();
}


const connection = new signalR.HubConnectionBuilder()
    .withUrl("/heightmap")
    .build();

connection.start().catch(err => console.log(err));

connection.on("heightmapData", (heightmapData) => {
    console.log('heightmap data received');
    console.log(heightmapData);
});

function sendPosition(position: Vector3): void {
    connection.send("positionChanged", position);
}