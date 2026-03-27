import { Injectable } from '@angular/core';
import { enviroment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of } from 'rxjs';
import { ISignIn } from '../interface/iSignIn';
import { IChanPass } from '../interface/iChangePass';

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
    return this.http.get<{ isAuthenticated: boolean }>(this.baseUrl + 'is-login').pipe(
      map(res => res.isAuthenticated),
      catchError(() => of(false))
    );
  }

  resetEmailSend(email: string): Observable<boolean> {
    const payload = {
      email: email
    }
    return this.http.post<{ isEmailSent: boolean }>(this.baseUrl + 'reset-email-sent', payload).pipe(
      map(res => res.isEmailSent),
      catchError(() => of(false))
    );
  }

  changePassword(changePass: IChanPass): Observable<string> {
    const payload = {
      changePasswordDto: {
        password: changePass.password,
        token: changePass.token
      }
    }
    
    return this.http.post<{ message: string }>(this.baseUrl + 'change-password', payload).pipe(
      map(res => res.message),
      catchError(err => of(err?.error?.message || 'Error changing password'))
    );
  }

   changePasswordManually(changePass: IChanPass): Observable<string> {
    const payload = {
      changePasswordDto: {
        email: changePass.email,
        password: changePass.password,
        token: changePass.token
      }
    }
    
    return this.http.post<{ message: string }>(this.baseUrl + 'change-password-manually', payload).pipe(
      map(res => res.message),
      catchError(err => of(err?.error?.message || 'Error changing password'))
    );
  }


}
