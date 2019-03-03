import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JoinGameSessionMessage, DiamondSquareHeightmapRequestDto, StartGameSesionRequestDto } from '../_models/generatedDtos';
import { SessionService } from '../_services/session.service';
import { SignalRService } from '../_services/signalr.service';
import { HeightmapService } from '../_services/heightmap.service';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.css']
})
export class LobbyComponent implements OnInit {
  public ownName: string;
  public gameSessionId: string;
  public players: string[];

  constructor(private route: ActivatedRoute,
              private sessionService: SessionService,
              private signalrService: SignalRService,
              private router: Router,
              private heightmapService: HeightmapService) { }

  ngOnInit() {
    this.gameSessionId = this.route.snapshot.paramMap.get('sessionId');
    this.ownName = this.route.snapshot.paramMap.get('playerName');
    this.getPlayers(this.gameSessionId);
    this.subscribeToSignalrEvents();
    this.onJoinedLobby();
  }

  private getPlayers(sessionId: string) {
    this.sessionService.getPlayers(sessionId).subscribe(resp => {
      this.players = resp;
    });
  }

  private subscribeToSignalrEvents() {
    this.signalrService.playerJoined.subscribe(player => {
      this.players.push(player);
    });

    this.signalrService.receiveTerrainDataIdReceived.subscribe(terrainDataId => {
      this.router.navigate([`/game/${terrainDataId}`]);
    });
  }

  private onJoinedLobby() {
    const message = new JoinGameSessionMessage({
      playerName: this.ownName,
      sessionId: this.gameSessionId
    });

    this.signalrService.sendJoinSession(message);
  }

  public onStart() {
    const heightmapRequestDto = new DiamondSquareHeightmapRequestDto({
      size: 513,
      offsetRange: 1500,
      offsetReductionRate: 0.4
    });

    this.heightmapService.generateDiamondSquareHeightmap(heightmapRequestDto).subscribe(terrainDataId => {
      const sessionStartRequestDto = new StartGameSesionRequestDto({
        sessionId: this.gameSessionId,
        terrainDataId
      });

      this.sessionService.startSession(sessionStartRequestDto).subscribe(res => {});
    });
  }
}
