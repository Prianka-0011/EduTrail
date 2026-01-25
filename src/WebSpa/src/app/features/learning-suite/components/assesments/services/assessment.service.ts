import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAssessment } from '../interface/iAssessment';


@Injectable({
  providedIn: 'root'
})
export class AssessmentService {

  baseUrl: string = enviroment.baseUrl + 'assessments/';
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

  updateAssessment(id: string, assessment: IAssessment): Observable<IAssessment> {
    const payload = {
      assessmentDetailDto: {
        title: assessment.title,
        description: assessment.description,
        courseId: assessment.courseId,
        availableCredit: assessment.availableCredit,
        maxScore: assessment.maxScore
      }
    };

    return this.http.put<IAssessment>(`${this.baseUrl}${id}`, payload);
  }

  createAssessment(assessment: IAssessment): Observable<IAssessment> {
    const payload = {
      assessmentDetailDto: {
        title: assessment.title,
        description: assessment.description,
        courseId: assessment.courseId,
        availableCredit: assessment.availableCredit,
        maxScore: assessment.maxScore
      }
    };

    return this.http.post<IAssessment>(this.baseUrl, payload);
  }

}
