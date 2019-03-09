import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DiamondSquareHeightmapRequestDto,
  FaultHeightmapRequestDto,
  HeightmapInfoResponseDto,
} from '../_models/generatedDtos';

@Injectable({
  providedIn: 'root'
})
export class HeightmapService {
  constructor(private http: HttpClient) { }

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
