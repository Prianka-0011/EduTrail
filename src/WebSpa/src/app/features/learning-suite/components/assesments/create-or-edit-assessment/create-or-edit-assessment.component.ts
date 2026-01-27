import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { AssessmentService } from '../services/assessment.service';
import { IAssessment } from '../interface/iAssessment';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-or-edit-assessment',
  imports: [CommonModule, FormsModule],
  templateUrl: './create-or-edit-assessment.component.html',
  styleUrl: './create-or-edit-assessment.component.scss'
})
export class CreateOrEditAssessmentComponent implements OnInit, OnChanges {

  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();
  @Input() courseId!: string;
  @Input() resetTrigger = false;

  @ViewChild('assessmentForm') assessmentForm!: NgForm;

  emptyGuid = '00000000-0000-0000-0000-000000000000';

  assessment: IAssessment = {
    id: this.emptyGuid,
    title: '',
    description: '',
    courseId: '',
    openDate: '',
    dueDate: '',
    maxScore: 0,
    availableCredit: 100
  };

  isTitleFocused = false;
  isDescriptionFocused = false;
  isOpenDateFocused = false;
  isDueDateFocused = false;
  isAvailableCreditFocused = false;

  constructor(
    private route: ActivatedRoute,
    private assessmentService: AssessmentService
  ) { }

  private toDateTimeLocal(value?: string | null): string {
    if (!value) return '';
    const date = new Date(value);
    // format as yyyy-MM-ddTHH:mm
    const pad = (n: number) => n.toString().padStart(2, '0');

    const year = date.getFullYear();
    const month = pad(date.getMonth() + 1); // months are 0-based
    const day = pad(date.getDate());
    const hours = pad(date.getHours());
    const minutes = pad(date.getMinutes());

    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const id = params['id'];

      if (id && id !== this.emptyGuid) {
        this.assessmentService.getAssessmentById(id).subscribe({
          next: data => {
            this.assessment = {
              ...data,
              openDate: this.toDateTimeLocal(data.openDate),
              dueDate: this.toDateTimeLocal(data.dueDate)
            };
          }
        });
      } else {
        this.assessment.courseId = this.courseId;
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['resetTrigger']?.currentValue && this.assessmentForm) {
      this.assessmentForm.resetForm({
        id: this.emptyGuid,
        title: '',
        description: '',
        courseId: this.courseId,
        openDate: '',
        dueDate: ''
      });
    }
  }

  onSubmit(form: NgForm) {
    if (!form.valid) return;

    const payload: IAssessment = {
      ...this.assessment,
      openDate: this.assessment.openDate ? new Date(this.assessment.openDate).toISOString() : undefined,
      dueDate: this.assessment.dueDate ? new Date(this.assessment.dueDate).toISOString() : undefined
    };

    if (this.assessment.id !== this.emptyGuid) {
      this.assessmentService.updateAssessment(this.assessment.id, payload).subscribe({
        next: () => this.saved.emit()
      });
    } else {
      this.assessmentService.createAssessment(payload).subscribe({
        next: () => this.saved.emit()
      });
    }
  }

  cancelForm() {
    this.cancel.emit();
  }
}