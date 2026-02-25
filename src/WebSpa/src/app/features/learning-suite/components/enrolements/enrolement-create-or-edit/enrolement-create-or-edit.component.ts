import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { IEnrolement } from '../interfaces/iEnrolement';
import { ActivatedRoute } from '@angular/router';
import { EnrolementService } from '../services/enrolement.service';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ITALabDay } from '../interfaces/iTALabDay';
import { ITALabSlot, LabMode } from '../interfaces/iTALabSlot';

@Component({
  selector: 'app-enrolement-create-or-edit',
  imports: [CommonModule, FormsModule],
  templateUrl: './enrolement-create-or-edit.component.html',
  styleUrl: './enrolement-create-or-edit.component.scss'
})

export class EnrolementCreateOrEditComponent implements OnInit {
  EMPTY_ID = '00000000-0000-0000-0000-000000000000';
  maxWeeklyHours = 10;
  LabMode = LabMode;

  enrolement: IEnrolement = this.getEmptyEnrolement();
  taLabDays: ITALabDay[] = [];

  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

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

    this.enrolementService.getCourseById(id).subscribe(data => {
      let enrolledDate = '';
      if (data.detailsDto?.enrolledDate) {
        enrolledDate = new Date(data.detailsDto.enrolledDate).toISOString().split('T')[0];
      }

      this.enrolement = {
        ...this.enrolement,
        detailsDto: {
          ...data.detailsDto,
          enrolledDate
        },
        users: data.users ?? []
      };

      // Initialize first TA day if TA
      if (this.enrolement.detailsDto.isTa && enrolledDate) {
        this.addLabDay(enrolledDate);
      }
    });
  }

  getEmptyEnrolement(): IEnrolement {
    return {
      detailsDto: {
        id: this.EMPTY_ID,
        courseOfferingId: '',
        studentId: '',
        studentName: '',
        enrolledDate: new Date().toISOString().split('T')[0],
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
      next: () => {
        this.toastr.success('Enrollment saved successfully!');
        this.saved.emit();
      },
      error: (err) => {
        if (err?.error?.message?.includes('already enrolled')) {
          this.toastr.warning('This student is already enrolled in this course offering.');
        } else {
          this.toastr.error('Something went wrong. Please try again.');
        }
      }
    });
  }

  onCancel(): void {
    this.cancel.emit();
  }

  getDayNameFromDate(date: string): string {
    return new Date(date).toLocaleDateString(undefined, { weekday: 'long' });
  }

  private generateGuid(): string {
    // Simple GUID generator
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, c => {
      const r = Math.random() * 16 | 0;
      const v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  addLabDay(date?: string) {
    const labDate = date || new Date().toISOString().split('T')[0];
    const newDayId = this.generateGuid();

    this.taLabDays.push({
      id: newDayId,
      enrollmentId: this.enrolement.detailsDto.id,
      labDate,
      isActive: true,
      slots: [
        {
          startTime: '',
          endTime: '',
          mode: LabMode.InPerson,
          remoteLink: '',
          isActive: true,
          talabDayId: newDayId
        }
      ]
    });
  }

  addLabSlot(day: ITALabDay) {
    const newSlot: ITALabSlot = {
      startTime: '',
      endTime: '',
      mode: LabMode.InPerson,
      remoteLink: '',
      isActive: true,
      talabDayId: day.id ?? this.generateGuid()
    };
    day.slots?.push(newSlot);
  }

  removeLabDay(index: number) {
    this.taLabDays.splice(index, 1);
  }

  removeLabSlot(day: ITALabDay, slotIndex: number) {
    day.slots?.splice(slotIndex, 1);
  }

  getTotalWeeklyHours(): number {
    let total = 0;
    this.taLabDays.forEach(day => {
      day.slots?.forEach(slot => {
        if (slot.startTime && slot.endTime) {
          const start = this.toMinutes(slot.startTime);
          const end = this.toMinutes(slot.endTime);
          if (end > start) total += (end - start) / 60;
        }
      });
    });
    return total;
  }

  toMinutes(time: string): number {
    const [h, m] = time.split(':').map(Number);
    return h * 60 + m;
  }

  // Whenever enrollment date changes, add a new day if it doesn’t exist
  onEnrollmentDateChange(date: string) {
    if (!this.taLabDays.find(d => d.labDate === date)) {
      this.addLabDay(date);
    }
  }
}