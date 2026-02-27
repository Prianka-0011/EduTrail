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

  isCourseFocused = false;
  isTermFocused = false;
  isInstructorFocused = false;

  constructor(
    private courseOfferingService: CourseOfferingService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    // this.loadDropdowns();
    this.loadCourseOffering();
  }

  // private loadDropdowns(): void {
  //   this.courseOfferingService.getCourses().subscribe(data => {
  //     this.courseOffering.courses = data.courses ?? [];
  //     this.courseOffering.terms = data.terms ?? [];
  //     this.courseOffering.instructors = data.instructors ?? [];
  //   });
  // }

  private loadCourseOffering(): void {
    const id = this.route.snapshot.queryParamMap.get('id');
    console.log('Course Offering ID from query params:', id);
    if (!id) return;

    this.courseOfferingService.getCourseOfferingById(id).subscribe(data => {
      this.courseOffering = {
        ...this.courseOffering,
        detailDto: data.detailDto ?? {
          id: EMPTY_ID,
          courseId: '',
          termId: '',
          instructorId: null
        },
        terms: data.terms ?? [],
        courses: data.courses ?? [],
        instructors: data.instructors ?? []
      };
    });
  }

  getEmptyCourseOffering(): ICourseOffering {
    return {
      detailDto: {
        id: EMPTY_ID,
        courseId: '',
        termId: '',
        instructorId: null
      },
      courses: [],
      terms: [],
      instructors: []
    };
  }

  onSubmit(form: NgForm): void {
    if (form.invalid) return;

    const request$ =
      this.courseOffering.detailDto?.id === EMPTY_ID
        ? this.courseOfferingService.createCourseOffering(this.courseOffering)
        : this.courseOfferingService.updateCourseOffering(this.courseOffering);

    request$.subscribe(() => {
      this.saved.emit();
    });
  }

  onCancel(): void {
    this.cancel.emit();
  }
}