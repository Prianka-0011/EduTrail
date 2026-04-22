import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { IWeeklyLabRequest, IWeeklyLabRequestDetail } from '../interfaces/IWeeklyLabRequestDetail';
import { enviroment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class HelpRequestDashboardService {

  private baseUrl = enviroment.baseUrl + 'helpRequestDashboards/';

  constructor(private http: HttpClient) { }

  getWeeklyDashboard(): Observable<IWeeklyLabRequest> {
    return this.http.get<IWeeklyLabRequest>(`${this.baseUrl}weekly-lab-requests`);
  }
}