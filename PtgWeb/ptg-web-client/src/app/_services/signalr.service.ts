import { Injectable } from '@angular/core';

import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';
import { JoinGameSessionMessage } from '../_models/generatedDtos';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private maxUnsuccessfulInvokeAttempts: 100;
  private hubConnection: HubConnection;

  public playerJoined = new Subject<string>();
  public playerLeft = new Subject<string>();
  public receiveTerrainDataIdReceived = new Subject<string>();

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  public sendJoinSession(message: JoinGameSessionMessage): Promise<any> {
    return this.invokeIfConnected('JoinSession', message);
  }

  private invokeIfConnected(methodName: string, message: any): Promise<any> {
    const promise = new Promise((resolve, reject) => {
      let invokeAttempts = 0;
      const stateCheckerId = setInterval(() => {
        console.log('checked');
        if (this.hubConnection.state === HubConnectionState.Connected) {
          this.hubConnection.invoke(methodName, message);
          clearInterval(stateCheckerId);
          resolve('ok');
        } else {
          invokeAttempts++;

          if (invokeAttempts >= this.maxUnsuccessfulInvokeAttempts) {
            reject('timeout');
          }
        }
      }, 100);
    });

    return promise;
  }

  private createConnection() {
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(environment.baseUrl + 'gameManager')
    .build();
  }

  private async startConnection() {
    this.hubConnection.start().then(() => {
      console.log('Hub connection started');
    });
  }

  // TODO register event for receiveTerrainDataGuid
  private registerOnServerEvents(): void {
    this.hubConnection.on('playerJoined', (player: string) => {
      this.playerJoined.next(player);
    });

    this.hubConnection.on('playerLeft', (player: string) => {
      this.playerLeft.next(player);
    });

    this.hubConnection.on('receiveTerrainDataId', (receiveTerrainDataId: string) => {
      this.receiveTerrainDataIdReceived.next(receiveTerrainDataId);
    });
  }
}
