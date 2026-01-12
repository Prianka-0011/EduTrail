import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { enviroment } from '../../../../../../environments/environment';
import { IQuestion } from '../interfaces/iQuestionDetail';
import { IGeneratedQuestion } from '../interfaces/iGeneratedQuestion';



@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  baseUrl = enviroment.baseUrl + "Questions/"

  constructor(private http: HttpClient) { }

  getAll(): Observable<IQuestion[]> {
    return this.http.get<IQuestion[]>(this.baseUrl);
  }

  getById(id: string): Observable<IQuestion> {
    return this.http.get<IQuestion>(`${this.baseUrl}${id}`);
  }

  create(question: IQuestion): Observable<IQuestion> {
    console.log('Creating question:', question);
    const payload = {
      questionDetailDto: {
        title: question.title,
        template: question.template,
        language: question.language,
        variationRules: question.variationRules?.map(rule => ({
          key: rule.key,
          options: rule.options
        }))
      }
    };

    return this.http.post<IQuestion>(this.baseUrl, payload);
  }

  update(id: string, question: IQuestion): Observable<IQuestion> {
    const payload = {
      questionDetailDto: {
        title: question.title,
        template: question.template,
        language: question.language,
        variationRules: question.variationRules?.map(rule => ({
          key: rule.key,
          options: rule.options
        }))
      }
    };

    return this.http.put<IQuestion>(`${this.baseUrl}/${id}`, payload);
  }

  getGeneratedQuestion(id: string): Observable<IGeneratedQuestion> {
    return this.http.get<IGeneratedQuestion>(`${this.baseUrl}generate/${id}`);
  }

  submitAnswer(questionId: string, answerOrder: string[]): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}submit/${questionId}`, { answerOrder });
  }
}
