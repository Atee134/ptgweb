import { Injectable } from '@angular/core';

import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';
import { JoinGameSessionMessage, MapLoadedMessage, LocationDto, LocationChangedMessage } from '../_models/generatedDtos';

@Injectable()
export class SignalRService {
  private maxUnsuccessfulInvokeAttempts: 100;
  private hubConnection: HubConnection;

  public playerJoined = new Subject<string>();
  public playerLeft = new Subject<string>();
  public terrainDataIdReceived = new Subject<string>();
  public playerIdReceived = new Subject<number>();
  public gameStarted = new Subject<LocationDto[]>();
  public locationsChanged = new Subject<LocationDto[]>();

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
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

  public sendJoinSession(message: JoinGameSessionMessage): Promise<any> {
    return this.invokeIfConnected('JoinSession', message);
  }

  public sendMapLoaded(message: MapLoadedMessage): Promise<any> {
    return this.invokeIfConnected('MapLoaded', message);
  }

  public sendLocationChanged(message: LocationChangedMessage) {
    return this.invokeIfConnected('LocationChanged', message);
  }

  private invokeIfConnected(methodName: string, message: any): Promise<any> {
    const promise = new Promise((resolve, reject) => {
      let invokeAttempts = 0;
      const stateCheckerId = setInterval(() => {
        if (this.hubConnection.state === HubConnectionState.Connected) {
          this.hubConnection.invoke(methodName, message);
          clearInterval(stateCheckerId);
          resolve('ok');
        } else {
          invokeAttempts++;

          if (invokeAttempts >= this.maxUnsuccessfulInvokeAttempts) {
            clearInterval(stateCheckerId);
            console.log('SignalR invoke attempts has reached the maximum, can not invoke hub method while not in the connected state');
            reject('timeout');
          }
        }
      }, 100);
    });

    return promise;
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on('playerJoined', (player: string) => {
      this.playerJoined.next(player);
    });

    this.hubConnection.on('playerLeft', (player: string) => {
      this.playerLeft.next(player);
    });

    this.hubConnection.on('receiveTerrainDataId', (terrainDataId: string) => {
      this.terrainDataIdReceived.next(terrainDataId);
    });

    this.hubConnection.on('receivePlayerId', (playerId: number) => {
      this.playerIdReceived.next(playerId);
    });

    this.hubConnection.on('startGame', (playerLocations: LocationDto[]) => {
      this.gameStarted.next(playerLocations);
    });

    this.hubConnection.on('locationsChanged', (playerLocations: LocationDto[]) => {
      this.locationsChanged.next(playerLocations);
    });
  }
}
