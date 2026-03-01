import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { IEnrolement } from '../interfaces/iEnrolement';

@Injectable({
  providedIn: 'root'
})
export class EnrolementService {

  baseUrl = enviroment.baseUrl + 'enrolements/';
  constructor(private http: HttpClient) { }
  getEnrolements(courseOfferingId: string): Observable<IEnrolement> {
    return this.http.get<IEnrolement>(this.baseUrl + "course-offerings/" + courseOfferingId);
  }

  getCourseById(id: string): Observable<IEnrolement> {
    return this.http.get<IEnrolement>(this.baseUrl + id);
  }

  createEnrolement(enrolement: IEnrolement): Observable<IEnrolement> {
    console.log("Creating enrolement:", enrolement);
    return this.http.post<IEnrolement>(this.baseUrl, {
      enrolementDto: {
        courseOfferingId: enrolement.detailsDto?.courseOfferingId,
        studentId: enrolement.detailsDto?.studentId,
        enrolledDate: enrolement.detailsDto?.enrolledDate,
        isTa: enrolement.detailsDto.isTa ?? false,
        totalWorkHoursPerWeek: enrolement.detailsDto.totalWorkHoursPerWeek,
      }
    });
  }

  updateEnrolement(enrolement: IEnrolement): Observable<IEnrolement> {
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
    return this.http.put<IEnrolement>(`${this.baseUrl}${enrolement.detailsDto.id}`, payload);
  }
}


