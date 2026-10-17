import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../../environments/environment';

import { IQuestionCatalogItem } from '../interfaces/IQuestionCatalogItem';
import { QuestionInstance, SubmissionResult } from '../interfaces/IPlayer';

// Reads the PrairieLearn-style question catalog (folders on the backend) and
// drives the interactive player (instances, submissions, correct answers).
@Injectable({
  providedIn: 'root'
})
export class QuestionCatalogService {
  private baseUrl = environment.baseUrl + 'QuestionCatalog';

  constructor(private http: HttpClient) { }

  getCatalog(): Observable<IQuestionCatalogItem[]> {
    return this.http.get<IQuestionCatalogItem[]>(this.baseUrl);
  }

  createInstance(slug: string): Observable<QuestionInstance> {
    return this.http.post<QuestionInstance>(`${this.baseUrl}/${slug}/instances`, {});
  }

  submitAnswer(instanceId: string, answers: Record<string, unknown>): Observable<SubmissionResult> {
    return this.http.post<SubmissionResult>(`${this.baseUrl}/instances/${instanceId}/submit`, { answers });
  }

  getCorrectAnswers(instanceId: string): Observable<Record<string, unknown>> {
    return this.http.get<Record<string, unknown>>(`${this.baseUrl}/instances/${instanceId}/correct`);
  }
}
