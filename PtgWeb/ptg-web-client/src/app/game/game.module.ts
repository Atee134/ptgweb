import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameComponent } from './game.component';
import { SignalRService } from '../_services/signalr.service';

@NgModule({
  declarations: [GameComponent],
  imports: [
    CommonModule,
  ],
  exports: [
    GameComponent
  ]
})
export class GameModule { }
