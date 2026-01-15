import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { enviroment } from '../../../../../../environments/environment';

import { IGeneratedQuestion } from '../interfaces/iGeneratedQuestion';
import { IQuestion } from '../interfaces/iQuestion';
import { IQuestionDetail } from '../interfaces/iQuestionDetail';



@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  baseUrl = enviroment.baseUrl + "Questions/"

  constructor(private http: HttpClient) { }

  getAll(): Observable<IQuestionDetail[]> {
    return this.http.get<IQuestionDetail[]>(this.baseUrl);
  }

  getById(id: string): Observable<IQuestion> {
    return this.http.get<IQuestion>(`${this.baseUrl}${id}`);
  }

  create(question: IQuestionDetail): Observable<IQuestionDetail> {
    console.log('Creating question:', question);
    const payload = {
      questionDetailDto: {
        title: question.title,
        template: question.template,
        language: question.language,
        questionTypeId: question.questionTypeId,
        assessmentId: question.assessmentId,
        variationRules: question.variationRules?.map(rule => ({
          key: rule.key,
          options: rule.options
        }))
      }
    };

    return this.http.post<IQuestionDetail>(this.baseUrl, payload);
  }

  update(id: string, question: IQuestionDetail): Observable<IQuestionDetail> {
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

    return this.http.put<IQuestionDetail>(`${this.baseUrl}/${id}`, payload);
  }

  getGeneratedQuestion(id: string): Observable<IGeneratedQuestion> {
    return this.http.get<IGeneratedQuestion>(`${this.baseUrl}generate/${id}`);
  }

  submitAnswer(questionId: string, answerOrder: string[]): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}submit/${questionId}`, { answerOrder });
  }
}
