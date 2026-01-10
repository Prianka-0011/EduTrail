import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { enviroment } from '../../../../../../environments/environment';
import { IQuestion } from '../interfaces/iQuestion';
import { IGeneratedQuestion } from '../interfaces/IGeneratedQuestion';



@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  baseUrl = enviroment.baseUrl + "/Questions"

  constructor(private http: HttpClient) { }

  getAll(): Observable<IQuestion[]> {
    return this.http.get<IQuestion[]>(this.baseUrl);
  }

  getById(id: string): Observable<IQuestion> {
    return this.http.get<IQuestion>(`${this.baseUrl}/${id}`);
  }

  create(question: IQuestion): Observable<IQuestion> {
    return this.http.post<IQuestion>(this.baseUrl, question);
  }

  update(id: string, question: IQuestion): Observable<IQuestion> {
    return this.http.put<IQuestion>(`${this.baseUrl}/${id}`, question);
  }

  getGeneratedQuestion(id: string): Observable<IGeneratedQuestion> {
    return this.http.get<IGeneratedQuestion>(`${this.baseUrl}/generate/${id}`);
  }

  submitAnswer(questionId: string, answerOrder: string[]): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/submit/${questionId}`, { answerOrder });
  }
}
