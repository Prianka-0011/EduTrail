import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { Observable } from 'rxjs';
import { ICourse } from '../interfaces/iCourse';

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  constructor(private http: HttpClient) { }
  baseUrl = enviroment.baseUrl + 'courses/';

  getCourses(): Observable<ICourse[]> {
    return this.http.get<ICourse[]>(this.baseUrl);
  }

  getCourseById(id: string): Observable<ICourse> {
    return this.http.get<ICourse>(this.baseUrl + id);
  }

  createCourse(course: ICourse): Observable<ICourse> {
    console.log("Creating course:", course);
    return this.http.post<ICourse>(this.baseUrl, {
      courseDto: {
        courseCode: course.courseCode,
        courseName: course.courseName,
        institute: course.institute,
        timeZone: course.timeZone
      }
    });
  }

  updateCourse(course: ICourse): Observable<ICourse> {
    if (!course.id || course.id === '00000000-0000-0000-0000-000000000000') {
      throw new Error('Cannot update course with invalid ID');
    }

    return this.http.put<ICourse>(`${this.baseUrl}${course.id}`, {
      courseDto: {
        id: course.id,
        courseCode: course.courseCode,
        courseName: course.courseName,
        institute: course.institute,
        timeZone: course.timeZone
      }
    });
  }


}
