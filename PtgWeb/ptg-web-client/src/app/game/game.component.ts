
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GameInitializerService } from './game-initializer.service.js';


@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  @ViewChild('viewport') viewPort: any;

  constructor(
    private route: ActivatedRoute,
    private gameInitializerService: GameInitializerService) { }

  ngOnInit() {
    const terrainDataId = this.route.snapshot.paramMap.get('terrainDataId');
    const canvas = this.viewPort.nativeElement as HTMLCanvasElement;
    this.gameInitializerService.initializeGame(terrainDataId, canvas); // TODO add settings inside initializer to this constructor
  }
}
