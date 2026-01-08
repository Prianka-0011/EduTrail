import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';import { ICourse } from '../interfaces/iCourse';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { CourseService } from '../services/course.service';

@Component({
  selector: 'app-course-create-or-update',
  imports: [CommonModule,
    FormsModule],
  templateUrl: './course-create-or-update.component.html',
  styleUrl: './course-create-or-update.component.scss'
})
export class CourseCreateOrUpdateComponent implements OnInit, OnChanges {
  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();
  @Input() resetTrigger:boolean = false;

  @ViewChild('courseForm') courseForm!: NgForm;
  course: ICourse = {
    id: '00000000-0000-0000-0000-000000000000',
    courseCode: '',
    courseName: '',
    institute: '',
    timeZone: ''
  };
  constructor(private route: ActivatedRoute, private courseService: CourseService) { }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['resetTrigger'] && changes['resetTrigger'].currentValue && this.courseForm) {
      this.courseForm.resetForm({
        id: '00000000-0000-0000-0000-000000000000',
        courseCode: '',
        courseName: '',
        institute: '',
        timeZone: ''
      });
    }
  }

  isCourseCodeFocused = false;
  isCourseNameFocused = false;
  isInstituteFocused = false;
  isTimeZoneFocused = false;

  onFocus(input: any) {
    this.isCourseCodeFocused = true;
    this.isCourseNameFocused = true;
    this.isInstituteFocused = true;
    this.isTimeZoneFocused = true;
  }

  onBlur(input: any) {
    this.isCourseCodeFocused = false;
    this.isCourseNameFocused = false;
    this.isInstituteFocused = false;
    this.isTimeZoneFocused = false;
  }

  timezones: string[] = Intl.supportedValuesOf('timeZone');

  ngOnInit(): void {
 this.route.queryParams.subscribe(params => {
  const courseId = params['id'];

  if (courseId && courseId !== '00000000-0000-0000-0000-000000000000') {
    this.courseService.getCourseById(courseId).subscribe({
      next: data => this.course = data
    });
  } else {
    if (this.courseForm) this.courseForm.resetForm(this.course);
  }
});

  }

  onSubmit(form: NgForm) {
    if (!form.valid) return;

    if (this.course.id && this.course.id !== '00000000-0000-0000-0000-000000000000') {
      console.log("form.valid",this.course)
      this.courseService.updateCourse(this.course).subscribe({
        next: () => this.saved.emit(),
        error: (err) => console.error(err)
      });
    } else {
      this.course.id = '00000000-0000-0000-0000-000000000000';
      this.courseService.createCourse(this.course).subscribe({
        next: () => this.saved.emit(),
        error: (err) => console.error(err)
      });
    }
  }

  cancelForm() {
    this.cancel.emit();
  }

  generateId(): string {
    return Math.random().toString(36).substring(2, 9);
  }
}