import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { IEnrolement } from '../interfaces/iEnrolement';

@Injectable({
  providedIn: 'root'
})
export class EnrolementService {

  baseUrl =  enviroment.baseUrl+'/enrolements';
  constructor(private http: HttpClient)
   { }
     getEnrolements(courseOfferingId: string): Observable<IEnrolement> {
       return this.http.get<IEnrolement>(this.baseUrl + `?courseOfferingId=${courseOfferingId}`);
     }
   
     getCourseById(id: string): Observable<IEnrolement> {
       return this.http.get<IEnrolement>(this.baseUrl + id);
     }
   
     createEnrolement(enrolement: IEnrolement): Observable<IEnrolement> {
       console.log("Creating enrolement:", enrolement);
       return this.http.post<IEnrolement>(this.baseUrl, {
         enrolementDetailDto: {
           courseOfferingId: enrolement.detailDto.CourseOfferingId,
           studentId: enrolement.detailDto.studentId,
           enrollmentDate: enrolement.detailDto.enrollmentDate,
         }
       });
     }
   
     updateEnrolement(enrolement: IEnrolement): Observable<IEnrolement> {
       if (!enrolement.detailDto.id || enrolement.detailDto.id === '00000000-0000-0000-0000-000000000000') {
         throw new Error('Cannot update enrolement with invalid ID');
       }
   
       return this.http.put<IEnrolement>(`${this.baseUrl}${enrolement.detailDto.id}`, {
         enrolementDetailDto: {
           id: enrolement.detailDto.id,
          courseOfferingId: enrolement.detailDto.CourseOfferingId,
           studentId: enrolement.detailDto.studentId,
           enrollmentDate: enrolement.detailDto.enrollmentDate,
         }
       });
     }
   }
   

