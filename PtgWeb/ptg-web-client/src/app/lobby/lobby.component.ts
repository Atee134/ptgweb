import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlayerDto } from '../_models/generatedDtos';
import { SessionService } from '../_services/session.service';
import { SignalRService } from '../_services/signalr.service';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.css']
})
export class LobbyComponent implements OnInit {

  public gameSessionId: string;
  public players: PlayerDto[];

  constructor(private route: ActivatedRoute, private sessionService: SessionService, private signalrService: SignalRService) { }

  ngOnInit() {
    this.gameSessionId = this.route.snapshot.paramMap.get('id');
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
    // TODO add player name as input parameter to his component, then here pass a dto to signalrSerice containing the playername and sessionId as well
    this.signalrService.sendJoinSession(this.gameSessionId);
  }
}
