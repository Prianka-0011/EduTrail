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

  term: ITerm = {
    id: '00000000-0000-0000-0000-000000000000',
    name: '',
    year: 0,
    startDate: '',
    endDate: ''
  };

  isNameFocused = false;
  isYearFocused = false;
  isStartDateFocused = false;
  isEndDateFocused = false;

  constructor(
    private service: TermService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const termId = params['id'];

      if (termId && termId !== this.term.id) {
        this.service.getById(termId).subscribe({
          next: data => this.term = data
        });
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['resetTrigger']?.currentValue && this.termForm) {
      this.resetForm();
    }
  }

  onSubmit(form: NgForm): void {
    if (!form.valid) return;

    const isUpdate =
      this.term.id !== '00000000-0000-0000-0000-000000000000';

    if (isUpdate) {
      this.service.update(this.term).subscribe({
        next: () => this.saved.emit(),
        error: err => console.error(err)
      });
    } else {
      this.service.create(this.term).subscribe({
        next: () => this.saved.emit(),
        error: err => console.error(err)
      });
    }
  }

  cancelForm(): void {
    this.cancel.emit();
  }

  onNameFocus() {
    this.isNameFocused = true;
  }

  onNameBlur() {
    this.isNameFocused = false;
  }

  onYearFocus() {
    this.isYearFocused = true;
  }

  onYearBlur() {
    this.isYearFocused = false;
  }

  onStartDateFocus() {
    this.isStartDateFocused = true;
  }

  onStartDateBlur() {
    this.isStartDateFocused = false;
  }

  onEndDateFocus() {
    this.isEndDateFocused = true;
  }

  onEndDateBlur() {
    this.isEndDateFocused = false;
  }

  private resetForm(): void {
    this.termForm.resetForm({
      id: '00000000-0000-0000-0000-000000000000',
      name: '',
      year: 0,
      startDate: '',
      endDate: ''
    });
  }
}
