
import { Component, OnInit, ViewChild } from '@angular/core';
import { GameManagerService } from './game-manager.service.js';
// import { SignalRService } from 'src/app/shared/signalr.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  @ViewChild('viewport') viewPort: any;

  constructor(
    private gameManagerService: GameManagerService
) { }

  ngOnInit() {
    const terrainDataId = sessionStorage.getItem('terrainDataId');
    const sessionId = sessionStorage.getItem('sessionId');
    const canvas = this.viewPort.nativeElement as HTMLCanvasElement;
    // this.subscribeToSignalrEvents();
    this.gameManagerService.startGame(sessionId, terrainDataId, canvas); // TODO add settings inside initializer to this constructor
  }

  // private subscribeToSignalrEvents() {
  //   this.signalrService.playerIdReceived.subscribe(playerId => {
  //     const asd = playerId;
  //   });
  // }
}
