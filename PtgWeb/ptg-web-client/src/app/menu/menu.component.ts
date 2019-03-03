import { Component, OnInit } from '@angular/core';
import { SignalRService } from '../_services/signalr.service';
import { SessionService } from '../_services/session.service';
import { CreateGameSessionRequestDto, JoinGameSessionRequestDto } from '../_models/generatedDtos';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  public playerName: string;
  public gameSessionId: string;

  constructor(private sessionService: SessionService, private signalrService: SignalRService, private router: Router) { }

  ngOnInit() {
  }

  onCreateSession() {
    const requestDto = new CreateGameSessionRequestDto({
      playerName: this.playerName
    });

    this.sessionService.createSession(requestDto).subscribe(resp => {
      this.router.navigate([`/lobby/${resp}/${this.playerName}`]);
    });
  }

  onJoinSession() {
    const requestDto = new JoinGameSessionRequestDto({
      playerName: this.playerName,
      sessionId: this.gameSessionId
    });

    this.sessionService.joinSession(requestDto).subscribe(resp => {
      this.router.navigate([`/lobby/${resp}/${this.playerName}`]);
    });
  }

  // TODO observe a terrainDataId subject, when a receiveTerrainDataId function is called by back end through signalr
  // just route to game component from here, and pass it the terrainDataID, which it then uses to build a mesh and texture
}
