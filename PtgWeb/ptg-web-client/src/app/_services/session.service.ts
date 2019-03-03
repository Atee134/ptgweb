import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CreateGameSessionRequestDto, JoinGameSessionRequestDto, StartGameSesionRequestDto, PlayerDto } from '../_models/generatedDtos';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  constructor(private http: HttpClient) { }

  public createSession(requestDto: CreateGameSessionRequestDto): Observable<string> {
    return this.http.post<string>(`${environment.baseUrl}api/gameSession/create`, requestDto, {withCredentials: true});
  }

  public joinSession(requestDto: JoinGameSessionRequestDto): Observable<string> {
    return this.http.post<string>(`${environment.baseUrl}api/gameSession/join`, requestDto, {withCredentials: true});
  }

  public startSession(requestDto: StartGameSesionRequestDto): Observable<string> {
    return this.http.post<string>(`${environment.baseUrl}api/gameSession/start`, requestDto, {withCredentials: true});
  }

  public getPlayers(sessionId: string): Observable<PlayerDto[]> {
    return this.http.get<PlayerDto[]>(`${environment.baseUrl}api/gameSession/${sessionId}/players`);
  }
}
