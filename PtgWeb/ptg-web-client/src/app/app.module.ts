import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap';

import { AppComponent } from './app.component';
import { GameModule } from './game/game.module';
import { MenuComponent } from './menu/menu.component';
import { LobbyComponent } from './lobby/lobby.component';
import { AppRoutingModule } from './app-routing.module';
import { ErrorInterceptorProvider } from './_services/error.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    LobbyComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ButtonsModule.forRoot(),
    GameModule,
    AppRoutingModule,
  ],
  providers: [ErrorInterceptorProvider],
  bootstrap: [AppComponent]
})
export class AppModule { }
