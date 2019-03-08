import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
// import { GroundMesh, Scene, Mesh } from 'babylonjs';
import { environment } from 'src/environments/environment';
import { DiamondSquareHeightmapRequestDto,
  SplatmapDto,
  FaultHeightmapRequestDto,
  HeightmapInfoResponseDto,
} from '../_models/generatedDtos';

@Injectable({
  providedIn: 'root'
})
export class HeightmapService {
  constructor(private http: HttpClient) { }

  // createGround(terrainDataId: string, terrainOptions: any, scene: Scene): GroundMesh { // TODO add terrainOptions class
  //   const heightmapUrl = `${environment.baseUrl}api/heightmap/${terrainDataId}`;
  //   const ground = Mesh.CreateGroundFromHeightMap('myGround', heightmapUrl, 256, 256, 300, 0, 15, scene);
  //   ground.checkCollisions = true;

  //   return ground;
  // }


  getHeightmapInfo(terrainDataId: string): Observable<HeightmapInfoResponseDto> {
    return this.http.get<HeightmapInfoResponseDto>(`${environment.baseUrl}api/heightmap/${terrainDataId}/info`);
  }

  getHeightmapUrl(terrainDataId: string): string {
    return `${environment.baseUrl}api/heightmap/${terrainDataId}`;
  }

  getSplatmapUrl(terrainDataId: string): string {
    return `${environment.baseUrl}api/splatmap/${terrainDataId}`;
  }

  generateDiamondSquareHeightmap(requestDto: DiamondSquareHeightmapRequestDto): Observable<string> {
    return this.http.post<string>(`${environment.baseUrl}api/heightmap/diamondSquare`, requestDto);
  }

  generateFaultHeightmap(requestDto: FaultHeightmapRequestDto): Observable<string> {
    return this.http.post<string>(`${environment.baseUrl}api/heightmap/fault`, requestDto);
  }
}
