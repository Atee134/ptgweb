import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { GroundMesh, Scene, Mesh } from 'babylonjs';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class HeightmapService {
  constructor(private http: HttpClient) { }

  createGround(terrainDataId: string, terrainOptions: any, scene: Scene): GroundMesh { // TODO add terrainOptions class
    const heightmapUrl = `${environment.baseUrl}api/heightmap/${terrainDataId}`;
    const ground = Mesh.CreateGroundFromHeightMap('myGround', heightmapUrl, 256, 256, 300, 0, 15, scene);
    ground.checkCollisions = true;

    return ground;
  }

  getSplatmap(options: any): Observable<string> { // TODO add swagger generated dtos
    return this.http.post<string>('api/heightmap/diamondSquare', {options});
  }

  // TODO add swagger generated dtos
  generateDiamondSquareHeightmap(size: number, offsetRange: number, offsetReductionRate: number): Observable<string> {
    return this.http.post<string>(`${environment.baseUrl}api/heightmap/diamondSquare`, {size, offsetRange, offsetReductionRate});
  }
}
