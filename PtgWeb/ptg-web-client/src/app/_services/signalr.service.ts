import { Injectable } from '@angular/core';

import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';
import { JoinGameSessionMessage } from '../_models/generatedDtos';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection;

  public playerJoined = new Subject<string>();
  public receiveTerrainDataIdReceived = new Subject<string>();

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  public sendJoinSession(message: JoinGameSessionMessage) {
    this.hubConnection.invoke('JoinSession', message);
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
    this.hubConnection.on('playerJoined', (player: string) => {
      this.playerJoined.next(player);
    });

    this.hubConnection.on('receiveTerrainDataId', (receiveTerrainDataId: string) => {
      this.receiveTerrainDataIdReceived.next(receiveTerrainDataId);
    });
  }
}
