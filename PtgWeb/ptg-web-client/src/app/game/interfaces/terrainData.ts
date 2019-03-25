import { HeightmapInfoResponseDto } from 'src/app/_models/generatedDtos';

export interface TerrainData {
    terrain: BABYLON.DynamicTerrain;
    heightmapInfo: HeightmapInfoResponseDto;
  }
