import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enviroment } from '../../../../../../environments/environment';
import { Observable } from 'rxjs';
import { ICourse } from '../interfaces/ICourse';

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  constructor(private http: HttpClient) { }
  baseUrl = enviroment.baseUrl + 'courses/';
  getCourses() : Observable<ICourse[]> {
    return this.http.get<ICourse[]>(this.baseUrl);
  }
  createCourse(course: ICourse) : Observable<ICourse> {
    return this.http.post<ICourse>(this.baseUrl, course);
  }
}
