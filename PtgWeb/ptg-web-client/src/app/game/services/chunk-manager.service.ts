/// <reference types="babylonjs"/>
// tslint:disable-next-line:no-reference
/// <reference path="../extensions/babylon.dynamicTerrain.d.ts"/>

import '../extensions/babylon.dynamicTerrain.js';

import { Injectable } from '@angular/core';
import { HeightmapService } from '../../_services/heightmap.service';

@Injectable({
  providedIn: 'root'
})
export class ChunkManagerService {

  private scene: BABYLON.Scene;
  private baseChunkId: string;
  private terrain: BABYLON.DynamicTerrain;
  private terrainChunkRequestThreshold = 28;
  private terrainChunkSizeX: number;
  private terrainChunkSizeZ: number;
  private terrainChunkHalfSizeX: number;
  private terrainChunkHalfSizeZ: number;

  private currentChunkCoords = new BABYLON.Vector2(0, 0);
  private requestedChunks: BABYLON.Vector2[] = [];

  constructor(private heightmapService: HeightmapService) { }

  public initialize(terrain: BABYLON.DynamicTerrain, baseChunkId: string, scene: BABYLON.Scene) {
    this.terrain = terrain;
    this.baseChunkId = baseChunkId;
    this.scene = scene;
    this.terrainChunkSizeX = terrain.mapSubX;
    this.terrainChunkSizeZ = terrain.mapSubZ;
    this.terrainChunkHalfSizeX = terrain.mapSubX / 2;
    this.terrainChunkHalfSizeZ = terrain.mapSubZ / 2;



    window.addEventListener('keypress', (e) => {
      if (e.keyCode === 32) {
          console.log('Chunkcoord X: ' + this.currentChunkCoords.x + '   chunkcoord Z: ' + this.currentChunkCoords.y);
      }
  }, false);
  }

  public manageChunks(currentPosition: BABYLON.Vector3) {
    this.refreshChunkCoords(currentPosition);

    // Check position to request new chunks
    const chunksNeeded: BABYLON.Vector2[] = [];

    // X direction
    const upperXBoundary =
      this.getBoundaryValue(this.currentChunkCoords.x, this.terrainChunkHalfSizeX, this.terrainChunkHalfSizeX, true);

    if (currentPosition.x > upperXBoundary) {
      chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x + 1, this.currentChunkCoords.y));
    }

    const lowerXBoundary =
      this.getBoundaryValue(this.currentChunkCoords.x, this.terrainChunkHalfSizeX, this.terrainChunkHalfSizeX, false);

    if (currentPosition.x < lowerXBoundary) {
      chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x - 1, this.currentChunkCoords.y));
    }

    // Z direction
    const upperZBoundary =
      this.getBoundaryValue(this.currentChunkCoords.y, this.terrainChunkHalfSizeZ, this.terrainChunkHalfSizeZ, true);

    if (currentPosition.z > upperZBoundary) {
      if (chunksNeeded.length > 0) {
        const xDirection = chunksNeeded[0];
        chunksNeeded.push(new BABYLON.Vector2(xDirection.x, this.currentChunkCoords.y + 1));
      }
      chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x, this.currentChunkCoords.y + 1));
    }

    const lowerZBoundary =
    this.getBoundaryValue(this.currentChunkCoords.y, this.terrainChunkHalfSizeZ, this.terrainChunkHalfSizeZ, false);

    if (currentPosition.z < lowerZBoundary) {
      if (chunksNeeded.length > 0) {
        const xDirection = chunksNeeded[0];
        chunksNeeded.push(new BABYLON.Vector2(xDirection.x, this.currentChunkCoords.y - 1));
      }
      chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x, this.currentChunkCoords.y - 1));
    }

    for (const chunkNeeded of chunksNeeded) {
      this.requestChunk(chunkNeeded);
    }
  }

  private getBoundaryValue(currentChunkCoord: number, terrainChunkSize: number, terrainChunkHalfSize: number, upper: boolean): number {
    if (upper) {
      return currentChunkCoord * terrainChunkSize + terrainChunkHalfSize - this.terrainChunkRequestThreshold;
    } else {
      return currentChunkCoord * terrainChunkSize - terrainChunkHalfSize + this.terrainChunkRequestThreshold;
    }
  }

  private refreshChunkCoords(currentPosition: BABYLON.Vector3) {
    this.currentChunkCoords.x = Math.floor((currentPosition.x + this.terrainChunkHalfSizeX) / this.terrainChunkSizeX);
    this.currentChunkCoords.y = Math.floor((currentPosition.z + this.terrainChunkHalfSizeZ) / this.terrainChunkSizeZ);
  }

  private requestChunk(coords: BABYLON.Vector2) {
    const requestedChunk = this.requestedChunks.find(ch => ch.x === coords.x && ch.y === coords.y);

    if (!requestedChunk) {
      console.log('!!REQUESTED');
      // TODO request chunk from server
      this.requestedChunks.push(coords);

      const heightmapUrl = this.heightmapService.getHeightmapChunkUrl(this.baseChunkId, coords.x, coords.y);

      const heightmapOptions = {
        width: this.terrainChunkSizeX, height: this.terrainChunkSizeZ,
        subX: this.terrainChunkSizeX, subZ: this.terrainChunkSizeZ,
        onReady: this.attachMapDataToExisting.bind(this),
        minHeight: 0,
        maxHeight: 20
      };

      const mapData = new Float32Array(this.terrainChunkSizeX * this.terrainChunkSizeZ * 3);

      BABYLON.DynamicTerrain.CreateMapFromHeightMapToRef(heightmapUrl, heightmapOptions as any, mapData, this.scene);
    }
  }

  private attachMapDataToExisting(mapData: number[], mapSubX: number, mapSubZ: number): void {

    const firstLength = mapData.length;
    const result = new Float32Array(firstLength + this.terrain.mapData.length);

    result.set(mapData);
    result.set(this.terrain.mapData, firstLength);

    this.terrain.mapData = result;
  }
}
