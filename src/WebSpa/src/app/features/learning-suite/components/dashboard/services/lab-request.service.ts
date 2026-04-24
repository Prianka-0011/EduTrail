import { Injectable } from '@angular/core';
import { environment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IHelpRequest } from '../interfaces/IHelpRequest';

@Injectable({
  providedIn: 'root'
})
export class LabRequestService {

  baseUrl = environment.baseUrl + 'labRequests'
  constructor(private http: HttpClient) { }
 
  createHelpRequest(request: IHelpRequest): Observable<IHelpRequest> {
    const payload =
    {
      helpRequest:
      {
        id: request.detailsDto?.id,
        requestNumber: request.detailsDto?.requestNumber,
        zoomLink: request.detailsDto?.zoomLink,
        issueTitle: request.detailsDto?.issueTitle,
        issueDescription: request.detailsDto?.issueDescription,
        trySofar: request.detailsDto?.trySofar,
        studentId: request.detailsDto?.studentId,
        courseOfferingId: request.detailsDto?.courseOfferingId,
        requestedDate: request.detailsDto?.requestedDate,
      }
    }
    return this.http.post<IHelpRequest>(`${this.baseUrl}/help-request`, payload);
  }

  getAllLabRequest(courseOfferingId: string): Observable<IHelpRequest> {
  return this.http.get<IHelpRequest>(
    `${this.baseUrl}/help-request-list/${courseOfferingId}`
  );
}

 getLabRequestById(id: string): Observable<IHelpRequest> {
  return this.http.get<IHelpRequest>(
    `${this.baseUrl}/${id}`
  );
}

 getAllLabRequestCurrentUser(courseOfferingId: string): Observable<IHelpRequest> {
  return this.http.get<IHelpRequest>(
    `${this.baseUrl}/help-request-by-current-user-list/${courseOfferingId}`
  );
}

updateLabRequest(request: IHelpRequest) : Observable<IHelpRequest>
{
  const payload =
    {
      helpRequest:
      {
        id: request.detailsDto?.id,
        requestNumber: request.detailsDto?.requestNumber,
        studentId: request.detailsDto?.studentId,
        zoomLink: request.detailsDto?.zoomLink,
        issueTitle: request.detailsDto?.issueTitle,
        issueDescription: request.detailsDto?.issueDescription,
        trySofar: request.detailsDto?.trySofar,
        statusId: request.detailsDto?.statusId,
        courseOfferingId: request.detailsDto?.courseOfferingId,
        requestedDate: request.detailsDto?.requestedDate,
      }
    }
  return this.http.put<IHelpRequest>(`${this.baseUrl}/${request.detailsDto?.id}`, payload);
}
}
