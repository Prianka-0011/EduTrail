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
  getCourses(): Observable<ICourseOffering[]> {
    return this.http.get<ICourseOffering[]>(this.baseUrl);
  }

  getCourseById(id: string): Observable<ICourseOffering> {
    return this.http.get<ICourseOffering>(this.baseUrl + id);
  }

  createCourse(courseOffering: ICourseOffering): Observable<ICourseOffering> {
    console.log("Creating course:", courseOffering);
    return this.http.post<ICourseOffering>(this.baseUrl, {
      courseOfferingDetailDto: {
        courseId: courseOffering.detail.courseId,
        instructorId: courseOffering.detail.instructorId,
        termId: courseOffering.detail.termId,

      }
    });
  }

  updateCourse(courseOffering: ICourseOffering): Observable<ICourseOffering> {
    if (!courseOffering.detail.id || courseOffering.detail.id === '00000000-0000-0000-0000-000000000000') {
      throw new Error('Cannot update course with invalid ID');
    }

    return this.http.put<ICourseOffering>(`${this.baseUrl}${courseOffering.detail.id}`, {
      courseOfferingDetailDto: {
        id: courseOffering.detail.id,
        courseId: courseOffering.detail.courseId,
        instructorId: courseOffering.detail.instructorId,
        termId: courseOffering.detail.termId,
      }
    });
  }
}
