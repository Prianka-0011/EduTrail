import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FieldFeedback } from '../interfaces/IPlayer';

// Single-select multiple choice (pl-multiple-choice). Options come from the
// "options" attribute as a JSON array string.
@Component({
  selector: 'pl-multiple-choice',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="mc-container">
      <label *ngFor="let opt of parsedOptions" class="mc-option">
        <input type="radio" [name]="answersName" [value]="opt" [(ngModel)]="selected" (ngModelChange)="answerChange.emit($event)" />
        {{ opt }}
      </label>
      <div *ngIf="_feedback && !_feedback.isCorrect" class="msg err">{{ _feedback.message }}</div>
      <div *ngIf="_feedback && _feedback.isCorrect" class="msg ok">✓ Correct!</div>
    </div>
  `,
  styles: [`
    .mc-container { margin: 0.5rem 0; }
    .mc-option { display: block; margin: 0.25rem 0; cursor: pointer; }
    .mc-option input { margin-right: 0.5rem; }
    .msg { font-size: 0.85rem; }
    .msg.err { color: #dc3545; }
    .msg.ok { color: #198754; }
  `],
})
export class PlMultipleChoiceComponent {
  @Input() answersName = '';
  @Input() set options(val: string) {
    try {
      const parsed = JSON.parse(val);
      this.parsedOptions = Array.isArray(parsed) ? parsed.map(String) : [];
    } catch { this.parsedOptions = []; }
  }
  @Input() set feedback(val: FieldFeedback | null) { this._feedback = val; }
  @Output() answerChange = new EventEmitter<string>();

  parsedOptions: string[] = [];
  selected = '';
  _feedback: FieldFeedback | null = null;
}
