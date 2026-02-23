import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { IEnrolement } from '../interfaces/iEnrolement';
import { ActivatedRoute } from '@angular/router';
import { EnrolementService } from '../services/enrolement.service';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-enrolement-create-or-edit',
  imports: [CommonModule, FormsModule],
  templateUrl: './enrolement-create-or-edit.component.html',
  styleUrl: './enrolement-create-or-edit.component.scss'
})

export class EnrolementCreateOrEditComponent implements OnInit {
  EMPTY_ID = '00000000-0000-0000-0000-000000000000';

  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  enrolement: IEnrolement = this.getEmptyEnrolement();

  isStudentFocused = false;
  isDateFocused = false;

  constructor(
    private enrolementService: EnrolementService,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.loadEnrolement();
  }

  private loadEnrolement(): void {
    const id = this.route.snapshot.queryParamMap.get('id') ?? this.EMPTY_ID;
    if (!id) return;
    console.log('Enrolement ID from query params:', id);
    this.enrolementService.getCourseById(id).subscribe(data => {
      console.log('Enrolement data from service:', data);
      this.enrolement = {
        ...this.enrolement,
        detailsDto: data.detailsDto ?? this.enrolement.detailsDto,
        users: data.users ?? []
      };
    });
  }

  getEmptyEnrolement(): IEnrolement {
    return {
      detailsDto: {
        id: this.EMPTY_ID,
        courseOfferingId: '',
        studentId: '',
        studentName: '',
        enrollmentDate: new Date().toISOString().split('T')[0],
        isTa: false
      },
      users: []
    };
  }

  onSubmit(form: NgForm): void {
    if (form.invalid) return;

    const courseOfferingId = this.route.snapshot.paramMap.get('courseOfferingId')!;
    this.enrolement.detailsDto!.courseOfferingId = courseOfferingId;

    const request$ =
      this.enrolement.detailsDto?.id === this.EMPTY_ID
        ? this.enrolementService.createEnrolement(this.enrolement)
        : this.enrolementService.updateEnrolement(this.enrolement);

    request$.subscribe({
      next: (data) => {
        this.toastr.success('Enrollment saved successfully!');
        this.saved.emit();
        console.log("Enrollment saved successfully:", data);
      },
      error: (err) => {
        if (err?.error?.message?.includes('already enrolled')) {
          this.toastr.warning('This student is already enrolled in this course offering.');
        } else {
          this.toastr.error('Something went wrong. Please try again.');
        }
        console.error("Error saving enrollment:", err);
      }
    });
  }

  onCancel(): void {
    this.cancel.emit();
  }
}