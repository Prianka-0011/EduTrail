import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { Observable } from 'rxjs';
import { ITerm, ITermDetails } from '../interfaces/iTerm';


@Injectable({
  providedIn: 'root'
})
export class TermService {

  baseUrl = enviroment.baseUrl + 'terms/';

  constructor(private http: HttpClient) { }

  getAll(): Observable<ITerm> {
    return this.http.get<ITerm>(this.baseUrl)
  }

  getById(id: string): Observable<ITerm> {
    return this.http.get<ITerm>(this.baseUrl + id)
  }

  create(term: ITerm): Observable<ITerm> {
    return this.http.post<ITerm>(this.baseUrl, {
      termDetailDto: {
        id: term.detailDto.id,
        name: term.detailDto.name,
        year: term.detailDto.year,
        startDate: term.detailDto.startDate,
        endDate: term.detailDto.endDate,
        termTypeId: term.detailDto.termTypeId
      
      }
    })
  }

  update(term: ITerm): Observable<ITerm> {
    return this.http.put<ITerm>(this.baseUrl + term.detailDto.id, {
      termDetailDto: {
        id: term.detailDto.id,
        name: term.detailDto.name,
        year: term.detailDto.year,
        startDate: term.detailDto.startDate,
        endDate: term.detailDto.endDate,
        termTypeId: term.detailDto.termTypeId
      }
    })
  }

}
