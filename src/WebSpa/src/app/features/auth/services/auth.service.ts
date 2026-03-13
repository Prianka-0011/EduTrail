import { Injectable } from '@angular/core';
import { enviroment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ISignIn } from '../interface/iSignIn';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = enviroment.baseUrl + "auths"
  constructor(private http: HttpClient) { }

  signIn(data: ISignIn): Observable<string> {
    const payload = {
      login: {
        email: data.email,
        password: data.password
      }
    };

    return this.http.post<string>(this.baseUrl + "/sign-in", payload);
  }
}
