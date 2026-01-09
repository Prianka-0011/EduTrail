import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { Observable } from 'rxjs';
import { ITerm } from '../interfaces/iTerm';
import { ICourse } from '../../courses/interfaces/iCourse';

@Injectable({
  providedIn: 'root'
})
export class TermService {

  baseUrl = enviroment.baseUrl + 'term/';

  constructor(private http: HttpClient) { }

  getAll(): Observable<ITerm[]> {
    return this.http.get<ITerm[]>(this.baseUrl)
  }

  getById(id: string): Observable<ITerm> {
    return this.http.get<ITerm>(this.baseUrl + id)
  }

  create(term: ITerm): Observable<ITerm> {
    return this.http.post<ITerm>(this.baseUrl, {
      termDto: {
        id: term.id,
        name: term.name,
        year: term.year,
        startDate: term.startDate,
        endDate: term.endDate
      }
    })
  }

  update(term: ITerm): Observable<ITerm> {
    return this.http.put<ITerm>(this.baseUrl + term.id, {
      termDto: {
        id: term.id,
        name: term.name,
        year: term.year,
        startDate: term.startDate,
        endDate: term.endDate
      }
    })
  }

}
