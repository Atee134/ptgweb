/// <reference types="babylonjs"/>
// tslint:disable-next-line:no-reference
/// <reference path="../extensions/babylon.dynamicTerrain.d.ts"/>

import '../extensions/babylon.dynamicTerrain.js';

import { Injectable } from '@angular/core';
import { HeightmapService } from '../../_services/heightmap.service';
import { TerrainData } from '../interfaces/terrainData.js';
import { HeightmapInfoResponseDto } from 'src/app/_models/generatedDtos.js';

@Injectable({
  providedIn: 'root'
})
export class ChunkManagerService {

  private scene: BABYLON.Scene;
  private baseChunkId: string;
  private terrain: BABYLON.DynamicTerrain;
  private terrainChunkRequestThreshold = 28;
  private terrainOverlappedSize: number;
  private terrainChunkRealSizeX: number;
  private terrainChunkRealSizeZ: number;
  private terrainChunkBaseSizeX: number;
  private terrainChunkBaseSizeZ: number;
  private terrainChunkHalfBaseSizeX: number;
  private terrainChunkHalfBaseSizeZ: number;

  private currentChunkCoords = new BABYLON.Vector2(0, 0);
  private requestedChunks: BABYLON.Vector2[] = [];

  constructor(private heightmapService: HeightmapService) { }

  public initialize(terrainData: TerrainData, baseChunkId: string, scene: BABYLON.Scene) {
    this.terrain = terrainData.terrain;
    this.baseChunkId = baseChunkId;
    this.scene = scene;
    this.terrainOverlappedSize = terrainData.heightmapInfo.overlappedSize;
    this.terrainChunkBaseSizeX = terrainData.heightmapInfo.width;
    this.terrainChunkBaseSizeZ = terrainData.heightmapInfo.height;
    this.terrainChunkRealSizeX = terrainData.heightmapInfo.width + terrainData.heightmapInfo.overlappedSize * 2;
    this.terrainChunkRealSizeZ = terrainData.heightmapInfo.height + terrainData.heightmapInfo.overlappedSize * 2;
    this.terrainChunkHalfBaseSizeX = this.terrainChunkBaseSizeX / 2;
    this.terrainChunkHalfBaseSizeZ = this.terrainChunkBaseSizeZ / 2;

    window.addEventListener('keypress', (e) => {
      if (e.keyCode === 32) {
          console.log('Chunkcoord X: ' + this.currentChunkCoords.x + '   chunkcoord Z: ' + this.currentChunkCoords.y);
      }
  }, false);
  }

  public manageChunks(currentPosition: BABYLON.Vector3) {
    this.refreshChunkCoords(currentPosition);



    // // Check position to request new chunks
    // const chunksNeeded: BABYLON.Vector2[] = [];

    // // X direction
    // const upperXBoundary =
    //   this.getBoundaryValue(this.currentChunkCoords.x, this.terrainChunkHalfSizeX, this.terrainChunkHalfSizeX, true);

    // if (currentPosition.x > upperXBoundary) {
    //   chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x + 1, this.currentChunkCoords.y));
    // }

    // const lowerXBoundary =
    //   this.getBoundaryValue(this.currentChunkCoords.x, this.terrainChunkHalfSizeX, this.terrainChunkHalfSizeX, false);

    // if (currentPosition.x < lowerXBoundary) {
    //   chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x - 1, this.currentChunkCoords.y));
    // }

    // // Z direction
    // const upperZBoundary =
    //   this.getBoundaryValue(this.currentChunkCoords.y, this.terrainChunkHalfSizeZ, this.terrainChunkHalfSizeZ, true);

    // if (currentPosition.z > upperZBoundary) {
    //   if (chunksNeeded.length > 0) {
    //     const xDirection = chunksNeeded[0];
    //     chunksNeeded.push(new BABYLON.Vector2(xDirection.x, this.currentChunkCoords.y + 1));
    //   }
    //   chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x, this.currentChunkCoords.y + 1));
    // }

    // const lowerZBoundary =
    // this.getBoundaryValue(this.currentChunkCoords.y, this.terrainChunkHalfSizeZ, this.terrainChunkHalfSizeZ, false);

    // if (currentPosition.z < lowerZBoundary) {
    //   if (chunksNeeded.length > 0) {
    //     const xDirection = chunksNeeded[0];
    //     chunksNeeded.push(new BABYLON.Vector2(xDirection.x, this.currentChunkCoords.y - 1));
    //   }
    //   chunksNeeded.push(new BABYLON.Vector2(this.currentChunkCoords.x, this.currentChunkCoords.y - 1));
    // }

    // for (const chunkNeeded of chunksNeeded) {
    //   this.requestChunk(chunkNeeded);
    // }
  }

  private getBoundaryValue(currentChunkCoord: number, terrainChunkSize: number, terrainChunkHalfSize: number, upper: boolean): number {
    if (upper) {
      return currentChunkCoord * terrainChunkSize + terrainChunkHalfSize - this.terrainChunkRequestThreshold;
    } else {
      return currentChunkCoord * terrainChunkSize - terrainChunkHalfSize + this.terrainChunkRequestThreshold;
    }
  }

  private refreshChunkCoords(currentPosition: BABYLON.Vector3) {
    const newChunkCoordX = Math.floor((currentPosition.x + this.terrainChunkHalfBaseSizeX) / this.terrainChunkBaseSizeX);
    const newChunkCoordY = Math.floor((currentPosition.z + this.terrainChunkHalfBaseSizeZ) / this.terrainChunkBaseSizeZ);

    if (newChunkCoordX !== this.currentChunkCoords.x || newChunkCoordY !== this.currentChunkCoords.y) {
      this.currentChunkCoords.x = newChunkCoordX;
      this.currentChunkCoords.y = newChunkCoordY;

      this.requestChunk(this.currentChunkCoords);
    }
  }

  private requestChunk(coords: BABYLON.Vector2) {

    console.log('REQUESTED CHUNK: X: ' + coords.x + ' Z: ' + coords.y);
    this.requestedChunks.push(coords);

    const heightmapUrl = this.heightmapService.getHeightmapChunkUrl(this.baseChunkId, coords.x, coords.y);

    const heightmapOptions = {
      width: this.terrainChunkRealSizeX, height: this.terrainChunkRealSizeZ,
      subX: this.terrainChunkRealSizeX, subZ: this.terrainChunkRealSizeZ,
      onReady: this.replaceMapdata.bind(this),
      minHeight: 0,
      maxHeight: 20,
      offsetX: coords.x * this.terrainChunkBaseSizeX,
      offsetZ: coords.y * this.terrainChunkBaseSizeZ,
    };

    const mapData = new Float32Array(this.terrainChunkRealSizeX * this.terrainChunkRealSizeZ * 3);

    BABYLON.DynamicTerrain.CreateMapFromHeightMapToRef(heightmapUrl, heightmapOptions as any, mapData, this.scene);
  }

  private replaceMapdata(mapData: number[], mapSubX: number, mapSubZ: number): void {
    this.terrain.precomputeNormalsFromMap = true;  // default = false
    this.terrain.mapData = mapData;
    this.terrain.update(true);
  }
}
