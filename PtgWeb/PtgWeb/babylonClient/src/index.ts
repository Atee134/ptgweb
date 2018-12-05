import { Scene, Engine, Vector3 } from 'babylonjs';
import * as signalR from "@aspnet/signalr";

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