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
  public isCreator: boolean;
  public ownName: string;
  public gameSessionId: string;
  public players: string[];

  constructor(private route: ActivatedRoute,
              private sessionService: SessionService,
              private signalrService: SignalRService,
              private router: Router
              ) { }

  ngOnInit() {
    this.gameSessionId = this.route.snapshot.paramMap.get('sessionId');
    this.ownName = this.route.snapshot.paramMap.get('playerName');
    this.isCreator = sessionStorage.getItem('SessionCreator') === 'true';
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

    this.signalrService.playerLeft.subscribe(player => {
      this.players = this.players.filter(p => p !== player);
    });

    this.signalrService.terrainDataIdReceived.subscribe(terrainDataId => {
      sessionStorage.setItem('terrainDataId', terrainDataId);
      sessionStorage.setItem('sessionId', this.gameSessionId);
      this.router.navigate([`/game`]);
    });
  }

  private onJoinedLobby() {
    const message = new JoinGameSessionMessage({
      playerName: this.ownName,
      sessionId: this.gameSessionId
    });

    this.signalrService.sendJoinSession(message);
  }

  public onBack() {
    this.sessionService.leaveSession().subscribe(resp => {
      this.router.navigate(['/menu']);
    });
  }
}
