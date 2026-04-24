import { Injectable } from '@angular/core';
import { environment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUser } from '../interfaces/IUser';
import { IUserDetail } from '../interfaces/IUserDetail';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl: string = environment.baseUrl + "users/";

  constructor(private http: HttpClient) { }

  getAll(): Observable<IUser> {
    return this.http.get<IUser>(this.baseUrl)
  }

  getById(id: string): Observable<IUser> {
    return this.http.get<IUser>(this.baseUrl + id)
  }

  create(user: IUserDetail): Observable<IUser> {
    return this.http.post<IUser>(this.baseUrl, {
      userDetailDto: {
        id: user.id,
        firstName: user.firstName,
        middleName: user.middleName,
        lastName: user.lastName,
        email: user.email,
        password: user.password,
        isActive: user.isActive,
        selectedRoleList: user.selectedRoleList
      }
    })
  }

  update(user: IUserDetail): Observable<IUser> {
    return this.http.put<IUser>(this.baseUrl + user.id, {
      userDetailDto: {
        id: user.id,
        firstName: user.firstName,
        middleName: user.middleName,
        lastName: user.lastName,
        email: user.email,
        password: user.password,
        isActive: user.isActive,
        selectedRoleList: user.selectedRoleList
      }
    })
  }

}
