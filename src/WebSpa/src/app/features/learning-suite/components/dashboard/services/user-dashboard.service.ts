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
    console.log("courseOfferingId", this.baseUrl+"/" + courseOfferingId)
    return this.http.get<IUserEnrolementByCourseOffering>(this.baseUrl+"/" + courseOfferingId)
  }

   updateEnrolement(enrolement: IUserEnrolementByCourseOffering): Observable<IUserEnrolementByCourseOffering> {
    if (!enrolement.detailsDto?.id || enrolement.detailsDto?.id === '00000000-0000-0000-0000-000000000000') {
      throw new Error('Cannot update enrolement with invalid ID');
    }

    const payload = {
      enrolementDto: {
        id: enrolement.detailsDto.id,
        courseOfferingId: enrolement.detailsDto.courseOfferingId,
        studentId: enrolement.detailsDto.studentId,
        studentName: enrolement.detailsDto.studentName,
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
    console.log(`${this.baseUrl}/${enrolement.detailsDto.id}`, "this")
    return this.http.put<IUserEnrolementByCourseOffering>(`${this.baseUrl}/${enrolement.detailsDto.id}`, payload);
  }

}
