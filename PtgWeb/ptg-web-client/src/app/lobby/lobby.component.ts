import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JoinGameSessionMessage } from '../_models/generatedDtos';
import { SessionService } from '../_services/session.service';
import { SignalRService } from '../_services/signalr.service';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.css']
})
export class LobbyComponent implements OnInit {
  public ownName: string;
  public gameSessionId: string;
  public players: string[];

  constructor(private route: ActivatedRoute, private sessionService: SessionService, private signalrService: SignalRService) { }

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
  }

  private onJoinedLobby() {
    const message = new JoinGameSessionMessage({
      playerName: this.ownName,
      sessionId: this.gameSessionId
    });

    this.signalrService.sendJoinSession(message);
  }
}
