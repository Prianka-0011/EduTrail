import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { QuestionService } from '../services/question.service';
import { IQuestionDetail } from '../interfaces/iQuestionDetail';
import { IDropdownItem } from '../../../../../shared/interface/iDropdownItem';

@Component({
  selector: 'app-question-create-or-update',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './question-create-or-update.component.html',
  styleUrls: ['./question-create-or-update.component.scss']
})
export class QuestionCreateOrUpdateComponent implements OnInit, OnChanges {

  @Input() questionId: string | null = null;
  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  question: IQuestionDetail = this.getEmptyQuestion();

  types: IDropdownItem[] = [];
  assesments: IDropdownItem[] = [];

  isTitleFocused = false;
  isLanguageFocused = false;
  isTemplateFocused = false;

  constructor(private questionService: QuestionService) {}

  ngOnInit(): void {
    this.loadDropdowns();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes['questionId'] &&
      this.questionId &&
      this.questionId !== '00000000-0000-0000-0000-000000000000'
    ) {
      this.loadQuestion(this.questionId);
    } else {
      this.question = this.getEmptyQuestion();
    }
  }

  getEmptyQuestion(): IQuestionDetail {
    return {
      id: '00000000-0000-0000-0000-000000000000',
      title: '',
      language: '',
      template: '',
      questionTypeId: null,
      assessmentId: null,
      variationRules: []
    };
  }

  loadDropdowns(): void {
    this.questionService.getCreateMeta().subscribe({
      next: res => {
        this.types = res.types;
        this.assesments = res.assesments;
      },
      error: err => console.error(err)
    });
  }

  loadQuestion(id: string): void {
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

  addRule(): void {
    this.question.variationRules.push({
      key: '',
      options: [],
      optionsStr: ''
    });
  }

  removeRule(index: number): void {
    this.question.variationRules.splice(index, 1);
  }

  onSubmit(form: NgForm): void {
    if (form.invalid) return;

    this.question.variationRules.forEach(rule => {
      rule.options =
        rule.optionsStr
          ?.split(',')
          .map(o => o.trim())
          .filter(Boolean) || [];
      delete (rule as any).optionsStr;
    });

    if (this.question.id === '00000000-0000-0000-0000-000000000000') {
      this.questionService.create(this.question).subscribe({
        next: () => this.saved.emit(),
        error: err => console.error(err)
      });
    } else {
      this.questionService.update(this.question.id!, this.question).subscribe({
        next: () => this.saved.emit(),
        error: err => console.error(err)
      });
    }
  }

  cancelForm(): void {
    this.cancel.emit();
  }
}
