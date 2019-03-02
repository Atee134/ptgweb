import { Injectable } from '@angular/core';

import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection;

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
  
  private registerOnServerEvents(): void {
    // this.hubConnection.on('FoodAdded', (data: any) => {
    //   this.foodchanged.next(data);
    // });

    // this.hubConnection.on('FoodDeleted', (data: any) => {
    //   this.foodchanged.next('this could be data');
    // });

    // this.hubConnection.on('FoodUpdated', (data: any) => {
    //   this.foodchanged.next('this could be data');
    // });

    // this.hubConnection.on('Send', (data: any) => {
    //   this.messageReceived.next(data);
    // });

    // this.hubConnection.on('newCpuValue', (data: number) => {
    //   this.newCpuValue.next(data);
    // });
  }
}
