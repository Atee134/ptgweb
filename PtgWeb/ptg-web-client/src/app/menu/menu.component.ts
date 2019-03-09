import { Component, OnInit } from '@angular/core';
import { SessionService } from '../_services/session.service';
import { CreateGameSessionRequestDto, JoinGameSessionRequestDto } from '../_models/generatedDtos';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  public joinMode: boolean;
  public playerName: string;
  public gameSessionId: string;

  constructor(
    private sessionService: SessionService,
    private router: Router,
    private alertify: AlertifyService) { }

  ngOnInit() {
  }

  onCreateSession() {
    const requestDto = new CreateGameSessionRequestDto({
      playerName: this.playerName
    });

    this.sessionService.createSession(requestDto).subscribe(resp => {
      sessionStorage.setItem('SessionCreator', 'true');
      this.router.navigate([`/lobby/${resp}/${this.playerName}`]);
    });
  }

  onJoinSession() {
    const requestDto = new JoinGameSessionRequestDto({
      playerName: this.playerName,
      sessionId: this.gameSessionId
    });

    this.sessionService.joinSession(requestDto).subscribe(resp => {
      sessionStorage.setItem('SessionCreator', 'false');
      this.router.navigate([`/lobby/${resp}/${this.playerName}`]);
    }, error => {
      this.alertify.error(error);
    });
  }

  changeJoinMode(value: boolean) {
    this.joinMode = value;
  }
}
