import * as signalR from "@aspnet/signalr";
import { Engine, Scene, UniversalCamera, StandardMaterial, Texture, Mesh, Vector3, Color4, DirectionalLight, Color3, TerrainMaterial } from 'babylonjs';
import 'babylonjs-materials';

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

    const options = {
        size: 513,
        offsetRange: 1500,
        offsetReductionRate: 0.4
    };

    fetch(baseApiUrl + 'heightmap/diamondSquare', {
        method: 'POST',
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(options)
      }).then(function(response) {
        return response.json();
      }).then(function(guid) {
        var ground = Mesh.CreateGroundFromHeightMap('myGround', baseApiUrl + 'heightmap/' + guid, 256, 256, 300, 0, 15, scene);
        ground.checkCollisions = true;
        ground.material = createTerrainMaterial(scene, baseApiUrl + 'splatmap/' + guid);
      });

    engine.runRenderLoop(renderLoop);
}

function createTerrainMaterial(scene: Scene, splatmapUrl: string): TerrainMaterial {
    	// Create terrain material
	var terrainMaterial = new TerrainMaterial("terrainMaterial", scene);
    terrainMaterial.specularColor = new Color3(0.5, 0.5, 0.5);
    terrainMaterial.specularPower = 64;
	
	// Set the mix texture (represents the RGB values)
    terrainMaterial.mixTexture = new Texture(splatmapUrl, scene);
	
	// Diffuse textures following the RGB values of the mix map
	// diffuseTexture1: Red
	// diffuseTexture2: Green
	// diffuseTexture3: Blue
    terrainMaterial.diffuseTexture1 = new Texture("textures/grass.jpg", scene);
    terrainMaterial.diffuseTexture2 = new Texture("textures/rock.jpg", scene);
    terrainMaterial.diffuseTexture3 = new Texture("textures/snow.jpg", scene);

    terrainMaterial.diffuseTexture1.uScale = terrainMaterial.diffuseTexture1.vScale = 30;
    terrainMaterial.diffuseTexture2.uScale = terrainMaterial.diffuseTexture2.vScale = 20;
    terrainMaterial.diffuseTexture3.uScale = terrainMaterial.diffuseTexture3.vScale = 20;
    
	// Bump textures according to the previously set diffuse textures
    // terrainMaterial.bumpTexture1 = new Texture("textures/floor_bump.png", scene);
    // terrainMaterial.bumpTexture2 = new Texture("textures/rockn.png", scene);
    // terrainMaterial.bumpTexture3 = new Texture("textures/grassn.png", scene);

    return terrainMaterial;
}

function createScene(): Scene {
    scene = new Scene(engine);
    scene.gravity = new Vector3(0, -0.2, 0);
    scene.collisionsEnabled = true;
    scene.clearColor = new Color4(0.275, 0.82, 1, 1);

    camera = createFreeCamera(scene);
    var light = new DirectionalLight("DirectionalLight", new Vector3(0, -1, 0), scene);
    light.intensity = 0.5;
    return scene;
}

function createFreeCamera(scene) {
    let cam = new UniversalCamera('universalCamera', new Vector3(0, 50, -10), scene);
    cam.attachControl(canvas);
    cam.ellipsoid = new Vector3(1, 1, 1);
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