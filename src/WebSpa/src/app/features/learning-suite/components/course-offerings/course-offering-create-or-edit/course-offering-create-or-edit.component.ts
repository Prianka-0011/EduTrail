import { Component } from '@angular/core';
import { ICourseOffering } from '../interfaces/iCourseOffering';
import { EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CourseOfferingService } from '../services/course-offering.service';
import { IDropdownItem } from '../../../../../shared/interface/iDropdownItem';


const EMPTY_ID = '00000000-0000-0000-0000-000000000000';
@Component({
  selector: 'app-course-offering-create-or-edit',
  imports: [CommonModule, FormsModule],
  templateUrl: './course-offering-create-or-edit.component.html',
  styleUrl: './course-offering-create-or-edit.component.scss'
})
export class CourseOfferingCreateOrEditComponent implements OnInit {

  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  courseOffering: ICourseOffering = this.getEmptyCourseOffering();

  courses: IDropdownItem[] = [];
  terms: IDropdownItem[] = [];
  instructors: IDropdownItem[] = [];

  isCourseFocused = false;
  isTermFocused = false;
  isInstructorFocused = false;

  constructor(
    private courseOfferingService: CourseOfferingService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const id = params['id'];
      if (!id) return;

      this.courseOfferingService.getCourseById(id).subscribe(data => {
        this.courseOffering = data;
        this.courses = data.courses ?? [];
        this.terms = data.terms ?? [];
        this.instructors = data.instructors ?? [];
        this
      });
    });
  }

  getEmptyCourseOffering(): ICourseOffering {
    return {
      detail: {
        id: EMPTY_ID,
        courseId: "",
        termId: "",
        instructorId:"",
      },
      courses: [],
      terms: [],
      instructors: []
    };
  }

  onSubmit(form: NgForm): void {
    if (form.invalid) return;

    const action =
      this.courseOffering.detail.id === EMPTY_ID
        ? this.courseOfferingService.createCourse(this.courseOffering)
        : this.courseOfferingService.updateCourse(this.courseOffering);

    action.subscribe(() => this.saved.emit());
  }

  cancelForm(): void {
    this.cancel.emit();
  }
}
