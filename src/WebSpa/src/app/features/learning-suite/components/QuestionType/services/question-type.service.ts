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
          Code: questionType.Code,
          Name: questionType.Name,
          Description: questionType.Description,
        }
      }
    return this.http.post<IQuestionType>(this.baseUrl, payload);
  }
}
