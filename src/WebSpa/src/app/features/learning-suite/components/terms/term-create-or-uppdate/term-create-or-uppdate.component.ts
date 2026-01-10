import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import { TermService } from '../services/term.service';
import { ITerm } from '../interfaces/iTerm';
import { ActivatedRoute } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';

const EMPTY_ID = '00000000-0000-0000-0000-000000000000';

@Component({
  selector: 'app-term-create-or-uppdate',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './term-create-or-uppdate.component.html',
  styleUrl: './term-create-or-uppdate.component.scss'
})
export class TermCreateOrUppdateComponent implements OnInit, OnChanges {

  @Input() resetTrigger = false;
  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();
  @ViewChild('termForm') termForm!: NgForm;

  term: ITerm = this.getEmptyTerm();

  isNameFocused = false;
  isYearFocused = false;
  isStartDateFocused = false;
  isEndDateFocused = false;

  minEndDate = '';

  constructor(
    private service: TermService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const termId = params['id'];
      if (termId && termId !== EMPTY_ID) {
        this.service.getById(termId).subscribe(data => {
          this.term = {
            ...data,
            startDate: data.startDate ? data.startDate.split('T')[0] : '',
            endDate: data.endDate ? data.endDate.split('T')[0] : ''
          };
          console.log("Edit form", this.term)
          this.syncDerivedFields();
        });
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {

    if (changes['resetTrigger']?.currentValue && this.termForm) {
      console.log("resetTrigger")
      this.resetForm();
    }
  }

  onSubmit(form: NgForm): void {
    if (form.invalid || !this.isDateRangeValid()) return;

    const payload: ITerm = {
      ...this.term,
      startDate: new Date(this.term.startDate).toISOString(),
      endDate: new Date(this.term.endDate).toISOString()
    };

    const request$ =
      this.term.id === EMPTY_ID
        ? this.service.create(payload)
        : this.service.update(payload);

    request$.subscribe(() => {
      this.saved.emit();
      this.resetForm();
    });
  }


  cancelForm(): void {
    this.resetForm();
    this.cancel.emit();
  }

  onStartDateChange(): void {
    this.syncDerivedFields();
    if (this.term.endDate && !this.isDateRangeValid()) {
      this.term.endDate = '';
    }
  }

  syncDerivedFields(): void {
    if (this.term.startDate) {
      this.minEndDate = this.term.startDate;
      this.term.year = new Date(this.term.startDate).getFullYear();
    }
  }

  isDateRangeValid(): boolean {
    if (!this.term.startDate || !this.term.endDate) return true;
    return new Date(this.term.endDate) >= new Date(this.term.startDate);
  }


  resetForm(): void {
    this.term = this.getEmptyTerm();
    this.minEndDate = '';
    this.termForm.resetForm(this.term);
  }

  getEmptyTerm(): ITerm {
    return {
      id: EMPTY_ID,
      name: '',
      year: 0,
      startDate: '',
      endDate: ''
    };
  }
}
