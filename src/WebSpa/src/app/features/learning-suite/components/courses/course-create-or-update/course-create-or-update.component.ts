import { Component, OnInit } from '@angular/core';
import { ICourse } from '../interfaces/ICourse';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-course-create-or-update',
  imports: [CommonModule,
    FormsModule],
  templateUrl: './course-create-or-update.component.html',
  styleUrl: './course-create-or-update.component.scss'
})
export class CourseCreateOrUpdateComponent implements OnInit {
  course: ICourse = {
    id: '',
    courseCode: '',
    courseName: '',
    institute: '',
    timezone: '',
  };

  timezones: string[] = Intl.supportedValuesOf('timeZone');

  ngOnInit(): void {
    // If editing, load course data here
    // Example:
    // this.course = this.courseService.getCourseById(courseId);
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      if (this.course.id) {
        console.log('Updating course:', this.course);
      } else {
        this.course.id = this.generateId();
        console.log('Creating course:', this.course);
      }
    }
  }

  cancel() {
    console.log('Cancelled');
  }

  generateId(): string {
    return Math.random().toString(36).substring(2, 9);
  }
}