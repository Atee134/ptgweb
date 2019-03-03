import { Injectable } from '@angular/core';

import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';
import { PlayerDto } from '../_models/generatedDtos';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection;

  public playerJoined = new Subject<PlayerDto>();

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  public sendJoinSession(sessionId: string) {
    this.hubConnection.invoke('JoinSession', sessionId);
  }

  private createConnection() {
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(environment.baseUrl + 'gameManager')
    .build();
  }

  private startConnection() {
    this.hubConnection.start().then(() => {
      console.log('Hub connection started');
    });
  }

  // TODO register event for receiveTerrainDataGuid
  private registerOnServerEvents(): void {
    this.hubConnection.on('playerJoined', (player: PlayerDto) => {
      this.playerJoined.next(player);
    });
  }
}
