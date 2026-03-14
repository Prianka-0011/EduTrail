import { Injectable } from '@angular/core';
import { enviroment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of } from 'rxjs';
import { ISignIn } from '../interface/iSignIn';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = enviroment.baseUrl + "auths/"
  constructor(private http: HttpClient) { }

  signIn(data: ISignIn): Observable<boolean> {
    const payload = {
      login: {
        email: data.email,
        password: data.password
      }
    };

    return this.http.post<boolean>(this.baseUrl + "sign-in", payload);
  }

  isLoggedIn(): Observable<boolean> {
    return this.http.get<{ isAuthenticated: boolean }>(this.baseUrl+'is-login').pipe(
      map(res => res.isAuthenticated),
      catchError(() => of(false))
    );
  }
}
