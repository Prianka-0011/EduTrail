import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAssessment } from '../interface/iAssessment';


@Injectable({
  providedIn: 'root'
})
export class AssessmentService {

  baseUrl: string = enviroment.baseUrl + 'assesments/';
  constructor(private http: HttpClient) { }

  getAssessments(): Observable<IAssessment[]> {
    return this.http.get<IAssessment[]>(this.baseUrl);
  }

  getAssessmentsByCourse(courseId: string): Observable<IAssessment[]> {
    return this.http.get<IAssessment[]>(this.baseUrl + 'course/' + courseId);
  }

  getAssessmentById(assesmentId: string): Observable<IAssessment> {
    return this.http.get<IAssessment>(this.baseUrl + assesmentId);
  }

  updateAssessment(assessment: IAssessment): Observable<IAssessment> {
    return this.http.put<IAssessment>(this.baseUrl + assessment.id, assessment);
  }

  createAssessment(assessment: IAssessment) : Observable<IAssessment>
  {
    console.log('Creating assessment:', assessment);
    return this.http.post<IAssessment>(this.baseUrl, assessment);
  }
}
