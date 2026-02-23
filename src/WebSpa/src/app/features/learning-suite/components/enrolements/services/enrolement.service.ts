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
        enrollmentDate: enrolement.detailsDto?.enrollmentDate,
        isTa: enrolement.detailsDto?.isTa ?? false
      }
    });
  }

  updateEnrolement(enrolement: IEnrolement): Observable<IEnrolement> {
    if (!enrolement.detailsDto?.id || enrolement.detailsDto?.id === '00000000-0000-0000-0000-000000000000') {
      throw new Error('Cannot update enrolement with invalid ID');
    }
    console.log("Updating enrolement:", enrolement);
    return this.http.put<IEnrolement>(`${this.baseUrl}${enrolement.detailsDto?.id}`, {
      enrolementDto: {
        id: enrolement.detailsDto?.id,
        courseOfferingId: enrolement.detailsDto?.courseOfferingId,
        studentId: enrolement.detailsDto?.studentId,
        enrollmentDate: enrolement.detailsDto?.enrollmentDate,
        isTa: enrolement.detailsDto?.isTa ?? false
      }
    });
  }
}


