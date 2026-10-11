import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../../environments/environment';

import { IQuestionCatalogItem } from '../interfaces/IQuestionCatalogItem';

// Reads the PrairieLearn-style question catalog (folders on the backend).
@Injectable({
  providedIn: 'root'
})
export class QuestionCatalogService {
  private baseUrl = environment.baseUrl + 'QuestionCatalog';

  constructor(private http: HttpClient) { }

  getCatalog(): Observable<IQuestionCatalogItem[]> {
    return this.http.get<IQuestionCatalogItem[]>(this.baseUrl);
  }
}
