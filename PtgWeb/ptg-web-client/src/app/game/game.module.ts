import { NgModule } from '@angular/core';
import { GameComponent } from './game.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [GameComponent],
  imports: [
    SharedModule
  ]
})
export class GameModule { }
