import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { Observable } from 'rxjs';
import { IQuestionType } from '../interface/iQuestionType';

@Injectable({
  providedIn: 'root'
})
export class QuestionTypeService {

  constructor(private http: HttpClient) { }
  baseUrl = enviroment.baseUrl + 'questionTypes/';
  getAll(): Observable<IQuestionType[]> {
    return this.http.get<IQuestionType[]>(this.baseUrl);
  }

  getById(id: string): Observable<IQuestionType> {
    return this.http.get<IQuestionType>(`${this.baseUrl}/${id}`);
  }

  create(questionType: IQuestionType): Observable<IQuestionType> {
      const payload = {
        QuestionTypeDetailDto: {
          Code: questionType.code,
          Name: questionType.name,
          Description: questionType.description,
        }
      }
    return this.http.post<IQuestionType>(this.baseUrl, payload);
  }

  update(id: string, questionType: IQuestionType): Observable<IQuestionType> {
    const payload = {
      QuestionTypeDetailDto: {
        Code: questionType.code,
        Name: questionType.name,
        Description: questionType.description,
      }
    }
    return this.http.put<IQuestionType>(`${this.baseUrl}/${id}`, payload);
  }

  
}
