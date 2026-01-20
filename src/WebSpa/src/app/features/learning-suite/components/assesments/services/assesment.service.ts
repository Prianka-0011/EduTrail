import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAssesment } from '../interface/iAssesment';

@Injectable({
  providedIn: 'root'
})
export class AssesmentService {

  baseUrl: string = enviroment.baseUrl + 'assesments/';
  constructor(private http: HttpClient) { }

  getAssesments(): Observable<IAssesment[]> {
    return this.http.get<IAssesment[]>(this.baseUrl);
  }

  getAssesmentsByCourse(courseId: string): Observable<IAssesment[]> {
    return this.http.get<IAssesment[]>(this.baseUrl + 'course/' + courseId);
  }

  getAssesmentById(assesmentId: string): Observable<IAssesment> {
    return this.http.get<IAssesment>(this.baseUrl + assesmentId);
  }

  updateAssesment(assesment: IAssesment): Observable<IAssesment> {
    return this.http.put<IAssesment>(this.baseUrl + assesment.id, assesment);
  }

  createAssesment(assesment: IAssesment) : Observable<IAssesment>
  {
    return this.http.post<IAssesment>(this.baseUrl, assesment);
  }
}
