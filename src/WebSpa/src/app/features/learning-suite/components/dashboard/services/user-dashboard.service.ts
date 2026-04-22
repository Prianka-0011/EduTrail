import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICourseOfferingByUser } from '../interfaces/ICourseOfferingByUser';
import { IEnrolement } from '../../enrolements/interfaces/IEnrolement';
import { IUserEnrolementByCourseOffering } from '../interfaces/IUserEnrolementByCourseOffering';
import { IHelpRequest } from '../interfaces/IHelpRequest';
import { ICurrentLoginUserDetail } from '../interfaces/ICurrentLoginUserDetail';

@Injectable({
  providedIn: 'root'
})
export class UserDashboardService {

  baseUrl = enviroment.baseUrl + 'userDashboards'
  enrollmentBaseUrl = enviroment.baseUrl+"enrolements/"
  constructor(private http: HttpClient) { }
  getCourseOfferingByUser(): Observable<ICourseOfferingByUser> {
    return this.http.get<ICourseOfferingByUser>(this.baseUrl)
  }

  getEnrolementByCourseOfferingAndLogingUser(courseOfferingId: string): Observable<IUserEnrolementByCourseOffering> {
    console.log("courseOfferingId", this.baseUrl + "/" + courseOfferingId)
    return this.http.get<IUserEnrolementByCourseOffering>(this.baseUrl + "/" + courseOfferingId)
  }

  getTAAndLabHoursByCourseOffering(courseOfferingId: string): Observable<IUserEnrolementByCourseOffering> {
    console.log("courseOfferingId", this.baseUrl + "/ta-hours/" + courseOfferingId)
    return this.http.get<IUserEnrolementByCourseOffering>(this.baseUrl + "/ta-hours/" + courseOfferingId)
  }

  updateEnrolement(enrolement: IUserEnrolementByCourseOffering): Observable<IUserEnrolementByCourseOffering> {
    if (!enrolement.detailsDto?.id || enrolement.detailsDto?.id === '00000000-0000-0000-0000-000000000000') {
      throw new Error('Cannot update enrolement with invalid ID');
    }

    const payload = {
      enrolementDto: {
        id: enrolement.detailsDto.id,
        courseOfferingId: enrolement.detailsDto.courseOfferingId,
        userId: enrolement.detailsDto.userId,
        enrolledDate: enrolement.detailsDto.enrolledDate
          ? new Date(enrolement.detailsDto.enrolledDate).toISOString()
          : null,
        isActive: enrolement.detailsDto.isActive ?? true,
        isTa: enrolement.detailsDto.isTa ?? false,
        totalWorkHoursPerWeek: enrolement.detailsDto.totalWorkHoursPerWeek,
        months: enrolement.detailsDto.months?.map(m => ({
          id: m.id,
          month: m.month,
          year: m.year,
          enrollmentId: m.enrollmentId,
          weeks: m.weeks?.map(w => ({
            id: w.id,
            tALabMonthId: w.taLabMonthId,
            weekNumber: w.weekNumber,
            days: w.days?.map(d => ({
              id: d.id,
              labDate: d.labDate ? new Date(d.labDate).toISOString() : null,
              tALabWeekId: d.taLabWeekId,
              isActive: d.isActive,
              slots: d.slots?.map(s => ({
                id: s.id,
                startTime: s.startTime
                  ? s.startTime.length === 8
                    ? s.startTime
                    : s.startTime + ":00"
                  : null,
                endTime: s.endTime
                  ? s.endTime.length === 8
                    ? s.endTime
                    : s.endTime + ":00"
                  : null,
                tALabDayId: s.taLabDayId,
                mode: s.mode,
                remoteLink: s.remoteLink
              })) ?? []
            })) ?? []
          })) ?? []
        })) ?? []
      }
    };
    return this.http.put<IUserEnrolementByCourseOffering>(`${this.baseUrl}/${enrolement.detailsDto.id}`, payload);
  }

  getCurrentLoginUser(): Observable<ICurrentLoginUserDetail> {
    console.log(this.baseUrl + "current-login-user")
    return this.http.get<ICurrentLoginUserDetail>(this.baseUrl + "/current-login-user");
  }

  loadActiveUsers(courseOfferingId: string): Observable<IEnrolement>
  {
     return this.http.get<IEnrolement>(this.enrollmentBaseUrl + "active-ta/" + courseOfferingId);
  }
  
  logout(): Observable<boolean> {
    return this.http.post<boolean>(`${this.baseUrl}/logout`, {});
  }
}
