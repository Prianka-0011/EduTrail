import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { IEnrolement } from '../interfaces/iEnrolement';
import { ActivatedRoute } from '@angular/router';
import { EnrolementService } from '../services/enrolement.service';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ITALabDay } from '../interfaces/iTALabDay';
import { ITALabSlot, LabMode } from '../interfaces/iTALabSlot';
import { ITALabMonth } from '../interfaces/iTALabMonth';
import { ITALabWeek } from '../interfaces/iTALabWeek';

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

  selectedMonth: number | null = 1;
  selectedYear: number | null = new Date().getFullYear();
  months = [
    { value: 1, name: 'January' }, { value: 2, name: 'February' },
    { value: 3, name: 'March' }, { value: 4, name: 'April' },
    { value: 5, name: 'May' }, { value: 6, name: 'June' },
    { value: 7, name: 'July' }, { value: 8, name: 'August' },
    { value: 9, name: 'September' }, { value: 10, name: 'October' },
    { value: 11, name: 'November' }, { value: 12, name: 'December' }
  ];
  years: number[] = [];

  enrolement: IEnrolement = this.getEmptyEnrolement();
  taLabMonths: ITALabMonth[] = [];

  isStudentFocused = false;
  isDateFocused = false;
  isMonthFocused = false;
  isYearFocused = false;
  isLabDateFocused = false;
  isStartTimeFocused = false;
  isEndTimeFocused = false;
  isModeFocused = false;
  isRemoteLinkFocused = false;
  totalHoursPerWeekFocused = false;

  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  constructor(
    private enrolementService: EnrolementService,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    const currentYear = new Date().getFullYear();
    for (let i = currentYear - 5; i <= currentYear + 5; i++) this.years.push(i);
    this.selectedYear = currentYear;
    this.loadEnrolement();
  }

  private loadEnrolement(): void {
    const id = this.route.snapshot.queryParamMap.get('id');
    if (!id || id === this.EMPTY_ID) return;

    this.enrolementService.getCourseById(id).subscribe(data => {
      const enrolledDate = data.detailsDto?.enrolledDate
        ? new Date(data.detailsDto.enrolledDate).toISOString().split('T')[0]
        : '';
      this.enrolement = {
        ...this.enrolement,
        detailsDto: {
          ...data.detailsDto,
          enrolledDate
        },
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
        enrolledDate: new Date().toISOString().split('T')[0],
        isTa: false,
        totalWorkHoursPerWeek: this.maxWeeklyHours
      },
      users: []
    };
  }

  onSubmit(form: NgForm): void {
    if (form.invalid) return;

    console.log('Enrollment data to save:', this.enrolement);
    // const courseOfferingId = this.route.snapshot.paramMap.get('courseOfferingId')!;
    // this.enrolement.detailsDto.courseOfferingId = courseOfferingId;

    // const request$ =
    //   this.enrolement.detailsDto.id === this.EMPTY_ID
    //     ? this.enrolementService.createEnrolement(this.enrolement)
    //     : this.enrolementService.updateEnrolement(this.enrolement);

    // request$.subscribe({
    //   next: () => {
    //     this.toastr.success('Enrollment saved successfully');
    //     this.saved.emit();
    //   },
    //   error: err => {
    //     if (err?.error?.message?.includes('already enrolled')) {
    //       this.toastr.warning('This student is already enrolled');
    //     } else {
    //       this.toastr.error('Something went wrong');
    //     }
    //   }
    // });
  }

  onCancel(): void { this.cancel.emit(); }

  onMonthChange(): void {
    if (!this.selectedYear) this.selectedYear = new Date().getFullYear();
  }

  public getMonthName(monthNumber: number): string {
    const monthNames = [
      'January', 'February', 'March', 'April', 'May', 'June',
      'July', 'August', 'September', 'October', 'November', 'December'
    ];
    return monthNames[monthNumber - 1] || '';
  }

  addMonth(month: number, year: number): void {
    if (!month || !year) return;
    if (this.taLabMonths.some(m => m.month === month && m.year === year)) return;

    const newMonth: ITALabMonth = {
      id: this.generateGuid(),
      month,
      year,
      weeks: [],
      enrollmentId: this.enrolement.detailsDto.id,
      isCollapsed: false
    };

    for (let i = 1; i <= 5; i++) newMonth.weeks.push({ weekNumber: i, days: [] });

    this.taLabMonths.push(newMonth);
  }

  addLabDayToWeek(month: ITALabMonth, weekNumber: number, date?: string): void {
    // if (!date) date = this.getTodayLocal();
    const week = month.weeks.find(w => w.weekNumber === weekNumber);
    if (!week) return;

    const duplicate = week.days.some(d => d.labDate === date);
    if (duplicate) {
      this.toastr.warning('This date is already added in this week');
      return;
    }

    const dayId = this.generateGuid();
    week.days.push({
      id: dayId,
      // enrollmentId: this.enrolement.detailsDto.id,
      labDate: date,
      isActive: true,
      slots: [
        { startTime: '', endTime: '', mode: LabMode.InPerson, remoteLink: '', isActive: true, talabDayId: dayId }
      ]
    });
  }

  removeLabDayFromWeek(month: ITALabMonth, weekNumber: number, dayIndex: number): void {
    const week = month.weeks.find(w => w.weekNumber === weekNumber);
    if (!week) return;
    week.days.splice(dayIndex, 1);
  }

  addLabSlot(day: ITALabDay): void {
    day.slots?.push({
      startTime: '',
      endTime: '',
      mode: LabMode.InPerson,
      remoteLink: '',
      isActive: true,
      talabDayId: day.id!
    });
  }

  removeLabSlot(day: ITALabDay, index: number): void {
    day.slots?.splice(index, 1);
  }

  getDayName(date?: string): string {
    if (!date) return '';
    const [year, month, day] = date.split('-').map(Number);
    return new Date(year, month - 1, day).toLocaleDateString(undefined, { weekday: 'long' });
  }

  private toMinutes(time: string): number {
    const [h, m] = time.split(':').map(Number);
    return h * 60 + m;
  }

  private getTodayLocal(): string {
    const now = new Date();
    return `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}-${String(now.getDate()).padStart(2, '0')}`;
  }

  private generateGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, c => {
      const r = Math.random() * 16 | 0;
      const v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  private getWeekStart(date: string): Date {
    const d = new Date(date);
    const day = d.getDay();
    const diff = d.getDate() - day;
    return new Date(d.getFullYear(), d.getMonth(), diff);
  }

  getTotalWeeklyHours(week: ITALabWeek): number {
    if (!week || !week.days) return 0;

    let total = 0;

    week.days.forEach(day => {
      day.slots?.forEach(slot => {
        if (slot.startTime && slot.endTime) {
          const start = this.toMinutes(slot.startTime);
          const end = this.toMinutes(slot.endTime);
          if (end > start) total += (end - start) / 60; // convert minutes to hours
        }
      });
    });

    return total;
  }

  onLabDateChange(month: ITALabMonth, weekNumber: number, dayIndex: number, newDate?: string) {
    if (!newDate) return;

    const week = month.weeks.find(w => w.weekNumber === weekNumber);
    if (!week) return;

    const day = week.days[dayIndex];
    if (!day) return;

    const [yearStr, monthStr, dayStr] = newDate.split('-');
    const selectedYear = month.year;
    const selectedMonth = month.month;

    // Ensure date is in the same month/year
    if (parseInt(yearStr, 10) !== selectedYear || parseInt(monthStr, 10) !== selectedMonth) {
      this.toastr.warning(`Please select a date within ${this.getMonthName(selectedMonth)} ${selectedYear}`);
      day.labDate = '';
      return;
    }

    // Check for duplicate date within the same month
    const isDuplicate = month.weeks.some(w =>
      w.days.some(d => d !== day && d.labDate === newDate)
    );

    if (isDuplicate) {
      this.toastr.warning(`The date ${newDate} is already added in this month.`);
      day.labDate = '';
      return;
    }

    // Check if the selected date falls in the correct week
    const dateObj = new Date(newDate);
    const weekStart = this.getWeekStart(newDate);
    const weekEnd = new Date(weekStart);
    weekEnd.setDate(weekEnd.getDate() + 6);

    if (dateObj < weekStart || dateObj > weekEnd) {
      this.toastr.warning(`The date ${newDate} does not belong to Week ${weekNumber}.`);
      day.labDate = '';
      return;
    }

    day.labDate = newDate;
    day.dayName = this.getDayName(newDate);

    // Calculate total hours for the week and flag if over limit
    const totalHours = this.getTotalWeeklyHours(week);
    day.isOverHours = totalHours > this.maxWeeklyHours;
  }

  getMonthStart(year: number, month: number): Date {
    return new Date(year, month - 1, 1);
  }

  getDaysInMonth(year: number, month: number): number {
    return new Date(year, month, 0).getDate();
  }

  // Week starts on Sunday
  getWeekStartDate(year: number, month: number, weekNumber: number): string {
    const monthStart = new Date(year, month - 1, 1);
    const firstDayOfWeek = monthStart.getDay(); // 0=Sun

    let startDay: number;

    if (weekNumber === 1) {
      startDay = 1;
    } else {
      startDay = 1 + (7 - firstDayOfWeek) + (weekNumber - 2) * 7;
    }

    return new Date(year, month - 1, startDay)
      .toISOString()
      .split('T')[0];
  }

  getWeekEndDate(year: number, month: number, weekNumber: number): string {
    const daysInMonth = new Date(year, month, 0).getDate();
    const monthStart = new Date(year, month - 1, 1);
    const firstDayOfWeek = monthStart.getDay();

    let endDay: number;

    if (weekNumber === 1) {
      endDay = 7 - firstDayOfWeek;
    } else {
      endDay = 1 + (7 - firstDayOfWeek) + (weekNumber - 1) * 7 - 1;
    }

    endDay = Math.min(endDay, daysInMonth);

    return new Date(year, month - 1, endDay)
      .toISOString()
      .split('T')[0];
  }



}