import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  constructor(private http: HttpClient) { }

  public createSession(playerName: string): Observable<string> { // TODO add swagger generated dtos
    return this.http.post<string>(`${environment.baseUrl}api/gameSession/create`, {playerName});
  }
}
