import { Component, OnInit } from '@angular/core';
import { SignalRService } from '../_services/signalr.service';
import { SessionService } from '../_services/session.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  constructor(private sessionService: SessionService, private signalrService: SignalRService) { }

  ngOnInit() {
  }

  onSubmit() {
    console.log('buttonpressed');
    this.signalrService.sendJoinSession('whatever');
  }

  createSession() {
    this.sessionService.createSession('asdsad').subscribe(resp => {
      console.log(resp);
    });
  }

  // TODO observe a terrainDataId subject, when a receiveTerrainDataId function is called by back end through signalr
  // just route to game component from here, and pass it the terrainDataID, which it then uses to build a mesh and texture
}
