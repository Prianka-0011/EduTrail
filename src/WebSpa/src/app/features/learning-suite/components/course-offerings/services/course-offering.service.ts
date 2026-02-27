import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICourseOffering } from '../interfaces/iCourseOffering';

@Injectable({
  providedIn: 'root'
})
export class CourseOfferingService {

  baseUrl = enviroment.baseUrl + 'courseOfferings/';
  constructor(private http: HttpClient) { }
  
  getCourseOfferings(): Observable<ICourseOffering> {
    return this.http.get<ICourseOffering>(this.baseUrl);
  }

  getCourseOfferingById(id: string): Observable<ICourseOffering> {
    return this.http.get<ICourseOffering>(this.baseUrl + id);
  }

  createCourseOffering(courseOffering: ICourseOffering): Observable<ICourseOffering> {
    console.log("Creating course:", courseOffering);
    return this.http.post<ICourseOffering>(this.baseUrl, {
      courseOfferingDetailDto: {
        courseId: courseOffering.detailDto?.courseId,
        instructorId: courseOffering.detailDto?.instructorId,
        termId: courseOffering.detailDto?.termId,

      }
    });
  }

  updateCourseOffering(courseOffering: ICourseOffering): Observable<ICourseOffering> {
    if (!courseOffering.detailDto?.id || courseOffering.detailDto?.id === '00000000-0000-0000-0000-000000000000') {
      throw new Error('Cannot update course offering with invalid ID');
    }

    return this.http.put<ICourseOffering>(`${this.baseUrl}${courseOffering.detailDto?.id}`, {
      courseOfferingDetailDto: {
        id: courseOffering.detailDto?.id,
        courseId: courseOffering.detailDto?.courseId,
        instructorId: courseOffering.detailDto?.instructorId,
        termId: courseOffering.detailDto?.termId,
      }
    });
  }
}
