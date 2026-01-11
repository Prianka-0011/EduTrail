import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { IQuestion } from '../interfaces/iQuestion';
import { QuestionService } from '../services/question.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-question-create-or-update',
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './question-create-or-update.component.html',
  styleUrls: ['./question-create-or-update.component.scss']
})
export class QuestionCreateOrUpdateComponent implements OnChanges {
  @Input() questionId: string | null = null;
  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  question: IQuestion = this.getEmptyQuestion();

  isTitleFocused = false;
  isLanguageFocused = false;
  isTemplateFocused = false;

  constructor(private questionService: QuestionService) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['questionId'] && this.questionId && this.questionId !== '00000000-0000-0000-0000-000000000000') {
      this.loadQuestion(this.questionId);
    } else if (this.questionId === '00000000-0000-0000-0000-000000000000') {
      this.question = this.getEmptyQuestion();
    }
  }

  getEmptyQuestion(): IQuestion {
    return {
      id: '00000000-0000-0000-0000-000000000000',
      title: '',
      language: '',
      template: '',
      variationRules: []
    };
  }

  loadQuestion(id: string) {
    this.questionService.getById(id).subscribe({
      next: data => {
        this.question = data;
        this.question.variationRules?.forEach(rule => {
          rule.optionsStr = rule.options?.join(', ') || '';
        });
      },
      error: err => console.error(err)
    });
  }

  addRule() {
    if (!this.question.variationRules) this.question.variationRules = [];
    this.question.variationRules.push({
      key: '',
      options: [],
      optionsStr: ''
    });
  }

  removeRule(index: number) {
    this.question.variationRules?.splice(index, 1);
  }

  onSubmit(form: NgForm) {
    if (form.invalid) return;

    this.question.variationRules?.forEach(rule => {
      rule.options = rule.optionsStr?.split(',').map(opt => opt.trim()).filter(opt => opt) || [];
      delete (rule as any).optionsStr;
    });

    if (this.question.id === '00000000-0000-0000-0000-000000000000') {
      this.questionService.create(this.question).subscribe({
        next: () => this.saved.emit(),
        error: err => console.error(err)
      });
    } else {
      this.questionService.update(this.question.id, this.question).subscribe({
        next: () => this.saved.emit(),
        error: err => console.error(err)
      });
    }
  }

  cancelForm() {
    this.cancel.emit();
  }
}
