import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { Observable } from 'rxjs';
import { ITerm } from '../interfaces/iTerm';

@Injectable({
  providedIn: 'root'
})
export class TermService {

  baseUrl = enviroment.baseUrl+'term/';

  constructor(private http : HttpClient) { }
  getAll():Observable<ITerm[]>
  {
    return this.http.get<ITerm[]>(this.baseUrl)
  }

}
