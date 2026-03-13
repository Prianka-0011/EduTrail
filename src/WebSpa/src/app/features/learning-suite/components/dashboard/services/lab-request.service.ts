import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICourseOfferingByUser } from '../interfaces/iCourseOfferingByUser';
import { IEnrolement } from '../../enrolements/interfaces/iEnrolement';
import { IUserEnrolementByCourseOffering } from '../interfaces/iUserEnrolementByCourseOffering';
import { IHelpRequest } from '../interfaces/iHelpRequest';

@Injectable({
  providedIn: 'root'
})
export class LabRequestService {

  baseUrl = enviroment.baseUrl + 'labRequests'
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


}
