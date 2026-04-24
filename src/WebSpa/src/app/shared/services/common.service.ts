import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { ICurrentLoginUserDetail } from '../../features/learning-suite/components/dashboard/interfaces/ICurrentLoginUserDetail';


@Injectable({
  providedIn: 'root'
})
export class CommonService {

  baseUrl = environment.baseUrl + 'userDashboards'
  constructor(private http: HttpClient) { }

  getCurrentLoginUser(): Observable<ICurrentLoginUserDetail>
  {
    console.log(this.baseUrl+"current-login-user")
    return this.http.get<ICurrentLoginUserDetail>(this.baseUrl+"/current-login-user");
  } 
  
  logout(): Observable<boolean> {
    return this.http.post<boolean>(`${this.baseUrl}/logout`, {});
  }

}
