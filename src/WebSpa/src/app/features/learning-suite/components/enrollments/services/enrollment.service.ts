import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { IEnrollment } from '../interfaces/IEnrollment';

@Injectable({
  providedIn: 'root'
})
export class EnrollmentService {

  baseUrl = enviroment.baseUrl + 'enrollments/';
  constructor(private http: HttpClient) { }
  getEnrollments(courseOfferingId: string): Observable<IEnrollment> {
    return this.http.get<IEnrollment>(this.baseUrl + "course-offerings/" + courseOfferingId);
  }

  getEnrollmentById(id: string): Observable<IEnrollment> {
    return this.http.get<IEnrollment>(this.baseUrl + id);
  }

  createEnrollment(enrolement: IEnrollment): Observable<IEnrollment> {
    console.log("Creating enrolement:", enrolement);
    return this.http.post<IEnrollment>(this.baseUrl, {
      enrolementDto: {
        courseOfferingId: enrolement.detailsDto?.courseOfferingId,
        userId: enrolement.detailsDto?.userId,
        enrolledDate: enrolement.detailsDto?.enrolledDate,
        isTa: enrolement.detailsDto.isTa ?? false,
        totalWorkHoursPerWeek: enrolement.detailsDto.totalWorkHoursPerWeek,
      }
    });
  }

  updateEnrollment(enrolement: IEnrollment): Observable<IEnrollment> {
    if (!enrolement.detailsDto?.id || enrolement.detailsDto?.id === '00000000-0000-0000-0000-000000000000') {
      throw new Error('Cannot update enrolement with invalid ID');
    }

    const payload = {
      enrolementDto: {
        id: enrolement.detailsDto.id,
        courseOfferingId: enrolement.detailsDto.courseOfferingId,
        userId: enrolement.detailsDto.userId,
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
    return this.http.put<IEnrollment>(`${this.baseUrl}${enrolement.detailsDto.id}`, payload);
  }

  bulkCreateEnrollments(file: File, courseOfferingId: string): Observable<any> {
    const formData = new FormData();

    formData.append('DetailDto.CourseOfferingId', courseOfferingId);
    formData.append('DetailDto.File', file);

    return this.http.post(this.baseUrl + 'bulk-upload', formData);
    // return this.http.post(this.baseUrl + 'bulk-upload', payload);
  }

}


