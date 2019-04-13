import { Component, OnInit, Input } from '@angular/core';
import { DiamondSquareHeightmapRequestDto,
  FaultHeightmapRequestDto,
  HeightmapType,
  StartGameSesionRequestDto,
  OpenSimplexRequestDto} from 'src/app/_models/generatedDtos';
import { HeightmapService } from 'src/app/_services/heightmap.service';
import { SessionService } from 'src/app/_services/session.service';

@Component({
  selector: 'app-lobby-settings',
  templateUrl: './lobby-settings.component.html',
  styleUrls: ['./lobby-settings.component.css']
})
export class LobbySettingsComponent implements OnInit {
  @Input() gameSessionId: string;
  public infiniteTerrain = false;
  public isRandom = true;
  public seed = 0;
  public diamondSquareRequestDto: DiamondSquareHeightmapRequestDto;
  public diamondSquareSizePossibleValues = [
    513,
    1025,
  ];
  public diamondSquareSizeIndex = 1;
  public faultRequestDto: FaultHeightmapRequestDto;
  public openSimplexRequestdto: OpenSimplexRequestDto;

  public selectedHeightmapType = HeightmapType.OpenSimplex;

  constructor(private heightmapService: HeightmapService, private sessionService: SessionService) { }

  ngOnInit() {
    this.initDtosWithDefaults();
  }

  public resetInfinite() {
    this.infiniteTerrain = false;
  }

  private initDtosWithDefaults() {
    this.diamondSquareRequestDto = new DiamondSquareHeightmapRequestDto({
      size: 513,
      offsetRange: 350,
      offsetReductionRate: 0.55
    });
    this.faultRequestDto = new FaultHeightmapRequestDto({
      width: 512,
      height: 512,
      iterationCount: 400,
      offsetPerIteration: 5
    });
    this.openSimplexRequestdto = new OpenSimplexRequestDto({
      width: 1024,
      height: 1024,
      overlappedSize: 0,
      seed: 134,
      scale: 0.02,
      octaves: 6,
      persistance: 0.5,
      lacunarity: 2,
      infinite: false,
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
      case HeightmapType.OpenSimplex: {
        return 'Open simplex noise';
      }
    }
  }

  public onStart() {

    if (this.infiniteTerrain) {
      sessionStorage.setItem('infinite', 'true');
    } else {
      sessionStorage.setItem('infinite', 'false');
    }

    switch (this.selectedHeightmapType) {
      case HeightmapType.Fault: {
        if (!this.isRandom) {
          this.faultRequestDto.seed = this.seed;
        } else {
          this.faultRequestDto.seed = null;
        }

        this.heightmapService.generateFaultHeightmap(this.faultRequestDto).subscribe(terrainDataId => {
          this.sendStartSession(terrainDataId);
        });
        break;
      }
      case HeightmapType.DiamondSquare: {
        if (!this.isRandom) {
          this.diamondSquareRequestDto.seed = this.seed;
        } else {
          this.diamondSquareRequestDto.seed = null;
        }

        this.heightmapService.generateDiamondSquareHeightmap(this.diamondSquareRequestDto).subscribe(terrainDataId => {
          this.sendStartSession(terrainDataId);
        });
        break;
      }
      case HeightmapType.OpenSimplex: {
        if (!this.isRandom) {
          this.openSimplexRequestdto.seed = this.seed;
        } else {
          this.openSimplexRequestdto.seed = null;
        }

        if (this.infiniteTerrain) {
          this.openSimplexRequestdto.infinite = true;
          this.openSimplexRequestdto.width = 256;
          this.openSimplexRequestdto.height = 256;
          this.openSimplexRequestdto.overlappedSize = 0;
          this.heightmapService.generateOpenSimplexHeightmap(this.openSimplexRequestdto).subscribe(terrainDataId => {
            this.sendStartSession(terrainDataId);
          });
        } else {
          this.openSimplexRequestdto.infinite = false;
          this.heightmapService.generateOpenSimplexHeightmap(this.openSimplexRequestdto).subscribe(terrainDataId => {
            this.sendStartSession(terrainDataId);
          });
        }
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
