import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FieldFeedback } from '../interfaces/IPlayer';

// Numeric answer input (pl-number-input). Emits its value on change and shows
// per-field feedback after grading.
@Component({
  selector: 'pl-number-input',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="pl-number-input">
      <input
        type="number"
        [(ngModel)]="value"
        (ngModelChange)="answerChange.emit($event)"
        [class.correct]="_feedback?.isCorrect === true"
        [class.incorrect]="_feedback?.isCorrect === false"
        [attr.placeholder]="label || answersName"
      />
      <span *ngIf="atol !== '0'" class="tol">(±{{ atol }})</span>
      <div *ngIf="_feedback && !_feedback.isCorrect" class="msg err">{{ _feedback.message }}</div>
      <div *ngIf="_feedback && _feedback.isCorrect" class="msg ok">✓ Correct!</div>
    </div>
  `,
  styles: [`
    .pl-number-input { margin: 0.5rem 0; }
    input { padding: 4px 8px; border: 1px solid #ccc; border-radius: 3px; }
    input.correct { border-color: #198754; background: #d1e7dd; }
    input.incorrect { border-color: #dc3545; background: #f8d7da; }
    .tol { font-size: 0.8rem; color: #666; margin-left: 0.4rem; }
    .msg { font-size: 0.85rem; margin-top: 0.25rem; }
    .msg.err { color: #dc3545; }
    .msg.ok { color: #198754; }
  `],
})
export class PlNumberInputComponent {
  @Input() answersName = '';
  @Input() label = '';
  @Input() atol = '0';
  @Input() rtol = '0';
  @Input() set feedback(val: FieldFeedback | null) { this._feedback = val; }
  @Output() answerChange = new EventEmitter<number>();

  value: number | null = null;
  _feedback: FieldFeedback | null = null;
}
