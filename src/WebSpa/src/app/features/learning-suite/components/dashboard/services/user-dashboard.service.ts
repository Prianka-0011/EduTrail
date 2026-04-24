import { Injectable } from '@angular/core';
import { environment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICourseOfferingByUser } from '../interfaces/ICourseOfferingByUser';
import { IUserEnrolementByCourseOffering } from '../interfaces/IUserEnrolementByCourseOffering';
import { IHelpRequest } from '../interfaces/IHelpRequest';
import { ICurrentLoginUserDetail } from '../interfaces/ICurrentLoginUserDetail';
import { IEnrollment } from '../../enrollments/interfaces/IEnrollment';

@Injectable({
  providedIn: 'root'
})
export class UserDashboardService {

  baseUrl = environment.baseUrl + 'userDashboards'
  enrollmentBaseUrl = environment.baseUrl+"enrollments/"
  constructor(private http: HttpClient) { }
  getCourseOfferingByUser(): Observable<ICourseOfferingByUser> {
    return this.http.get<ICourseOfferingByUser>(this.baseUrl)
  }

  getEnrollmentByCourseOfferingAndLoggedInUser(courseOfferingId: string): Observable<IUserEnrolementByCourseOffering> {
    console.log("courseOfferingId", this.baseUrl + "/" + courseOfferingId)
    return this.http.get<IUserEnrolementByCourseOffering>(this.baseUrl + "/" + courseOfferingId)
  }

  getTAAndLabHoursByCourseOffering(courseOfferingId: string): Observable<IUserEnrolementByCourseOffering> {
    console.log("courseOfferingId", this.baseUrl + "/ta-hours/" + courseOfferingId)
    return this.http.get<IUserEnrolementByCourseOffering>(this.baseUrl + "/ta-hours/" + courseOfferingId)
  }

  updateEnrollment(enrollment: IUserEnrolementByCourseOffering): Observable<IUserEnrolementByCourseOffering> {
    if (!enrollment.detailsDto?.id || enrollment.detailsDto?.id === '00000000-0000-0000-0000-000000000000') {
      throw new Error('Cannot update enrollment with invalid ID');
    }

    const payload = {
      enrolementDto: {
        id: enrollment.detailsDto.id,
        courseOfferingId: enrollment.detailsDto.courseOfferingId,
        userId: enrollment.detailsDto.userId,
        enrolledDate: enrollment.detailsDto.enrolledDate
          ? new Date(enrollment.detailsDto.enrolledDate).toISOString()
          : null,
        isActive: enrollment.detailsDto.isActive ?? true,
        isTa: enrollment.detailsDto.isTa ?? false,
        totalWorkHoursPerWeek: enrollment.detailsDto.totalWorkHoursPerWeek,
        months: enrollment.detailsDto.months?.map(m => ({
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
    return this.http.put<IUserEnrolementByCourseOffering>(`${this.baseUrl}/${enrollment.detailsDto.id}`, payload);
  }

  getCurrentLoginUser(): Observable<ICurrentLoginUserDetail> {
    console.log(this.baseUrl + "current-login-user")
    return this.http.get<ICurrentLoginUserDetail>(this.baseUrl + "/current-login-user");
  }

  loadActiveUsers(courseOfferingId: string): Observable<IEnrollment>
  {
     return this.http.get<IEnrollment>(this.enrollmentBaseUrl + "active-ta/" + courseOfferingId);
  }
  
  logout(): Observable<boolean> {
    return this.http.post<boolean>(`${this.baseUrl}/logout`, {});
  }
}
