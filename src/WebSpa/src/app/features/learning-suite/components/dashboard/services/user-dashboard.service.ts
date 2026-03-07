import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICourseOfferingByUser } from '../interfaces/iCourseOfferingByUser';
import { IEnrolement } from '../../enrolements/interfaces/iEnrolement';
import { IUserEnrolementByCourseOffering } from '../interfaces/iUserEnrolementByCourseOffering';

@Injectable({
  providedIn: 'root'
})
export class UserDashboardService {

  baseUrl = enviroment.baseUrl + 'userDashboards'
  constructor(private http: HttpClient) { }
  getCourseOfferingByUser(): Observable<ICourseOfferingByUser> {
    return this.http.get<ICourseOfferingByUser>(this.baseUrl)
  }

  getEnrolementByCourseOfferingAndLogingUser(courseOfferingId: string): Observable<IUserEnrolementByCourseOffering> {
    return this.http.get<IUserEnrolementByCourseOffering>(this.baseUrl+"/" + courseOfferingId)
  }

}
