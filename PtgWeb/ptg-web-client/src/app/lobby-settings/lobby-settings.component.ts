import { Component, OnInit, Input } from '@angular/core';
import { HeightmapService } from '../_services/heightmap.service';
import {
  DiamondSquareHeightmapRequestDto,
  FaultHeightmapRequestDto,
  HeightmapType,
  StartGameSesionRequestDto } from '../_models/generatedDtos';
import { SessionService } from '../_services/session.service';

@Component({
  selector: 'app-lobby-settings',
  templateUrl: './lobby-settings.component.html',
  styleUrls: ['./lobby-settings.component.css']
})
export class LobbySettingsComponent implements OnInit {
  @Input() gameSessionId: string;
  public diamondSquareRequestDto: DiamondSquareHeightmapRequestDto;
  public diamondSquareSizePossibleValues = [
    65,
    129,
    257,
    513,
    1025,
    2049,
  ];
  public diamondSquareSizeIndex = 3;
  public faultRequestDto: FaultHeightmapRequestDto;

  public selectedHeightmapType = HeightmapType.Fault;

  constructor(private heightmapService: HeightmapService, private sessionService: SessionService) { }

  ngOnInit() {
    this.initDtosWithDefaults();
  }

  private initDtosWithDefaults() {
    this.diamondSquareRequestDto = new DiamondSquareHeightmapRequestDto({
      size: 257,
      offsetRange: 350,
      offsetReductionRate: 0.55
    });
    this.faultRequestDto = new FaultHeightmapRequestDto({
      width: 256,
      height: 256,
      iterationCount: 500,
      offsetPerIteration: 5
    });
  }

  public onDiamondSquareSizeChanged() {
    this.diamondSquareRequestDto.size = this.diamondSquareSizePossibleValues[this.diamondSquareSizeIndex - 1];
  }

  get heightmapTypes(): HeightmapType[] {
    return Object.keys(HeightmapType).map(k => HeightmapType[k]);
  }

  public isSelectedType(heightmapType: string): boolean {
    return this.selectedHeightmapType.toLowerCase() === heightmapType.toLowerCase();
  }

  public getPrettyName(heightmapType: HeightmapType): string {
    switch (heightmapType) {
      case HeightmapType.Fault: {
        return 'Fault algorithm';
      }
      case HeightmapType.DiamondSquare: {
        return 'Diamond square algorithm';
      }
      case HeightmapType.PerlinNoise: {
        return 'Perlin noise';
      }
    }
  }

  public onStart() {
    switch (this.selectedHeightmapType) {
      case HeightmapType.Fault: {
        this.heightmapService.generateFaultHeightmap(this.faultRequestDto).subscribe(terrainDataId => {
          this.sendStartSession(terrainDataId);
        });
        break;
      }
      case HeightmapType.DiamondSquare: {
        this.heightmapService.generateDiamondSquareHeightmap(this.diamondSquareRequestDto).subscribe(terrainDataId => {
          this.sendStartSession(terrainDataId);
        });
        break;
      }
      case HeightmapType.PerlinNoise: {
        // TODO call perlin noise generation here
        break;
      }
    }
  }

  private sendStartSession(terrainDataId: string): void {
    const sessionStartRequestDto = new StartGameSesionRequestDto({
      sessionId: this.gameSessionId,
      terrainDataId
    });

    this.sessionService.startSession(sessionStartRequestDto).subscribe(res => {});
  }
}
