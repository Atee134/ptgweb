import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MenuComponent } from './menu.component';
import { LobbyComponent } from './lobby/lobby.component';
import { LobbySettingsComponent } from './lobby-settings/lobby-settings.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    MenuComponent,
    LobbyComponent,
    LobbySettingsComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule
  ]
})
export class MenuModule { }
