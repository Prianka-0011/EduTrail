import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { QuestionService } from '../services/question.service';
import { IDropdownItem } from '../../../../../shared/interface/iDropdownItem';
import { IQuestion } from '../interfaces/iQuestion';

const EMPTY_ID = '00000000-0000-0000-0000-000000000000';

@Component({
  selector: 'app-question-create-or-update',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './question-create-or-update.component.html',
  styleUrls: ['./question-create-or-update.component.scss']
})
export class QuestionCreateOrUpdateComponent implements OnInit {

  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  question: IQuestion = this.getEmptyQuestion();

  types: IDropdownItem[] = [];
  assesments: IDropdownItem[] = [];

  isTitleFocused = false;
  isLanguageFocused = false;
  isTemplateFocused = false;

  constructor(
    private questionService: QuestionService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const id = params['id'];
      if (!id) return;

      this.questionService.getById(id).subscribe({
        next: data => {
          this.question = data;

          if (!this.question.details) {
            this.question.details = {
              id: EMPTY_ID,
              title: '',
              language: '',
              template: '',
              questionTypeId: EMPTY_ID,
              assessmentId: EMPTY_ID,
              variationRules: []
            };
          }

          this.question.details.variationRules ??= [];

          this.types = data.types ?? [];
          this.assesments = data.assesments ?? [];

          this.question.details.variationRules.forEach(rule => {
            rule.optionsStr = rule.options?.join(', ') || '';
          });
        }
      });
    });
  }

  getEmptyQuestion(): IQuestion {
    return {
      details: {
        id: EMPTY_ID,
        title: '',
        language: '',
        template: '',
        questionTypeId: EMPTY_ID,
        assessmentId: EMPTY_ID,
        variationRules: []
      },
      types: [],
      assesments: []
    };
  }

  addRule(): void {
    if (!this.question.details) {
      this.question.details = {
        id: EMPTY_ID,
        title: '',
        language: '',
        template: '',
        questionTypeId: EMPTY_ID,
        assessmentId: EMPTY_ID,
        variationRules: []
      };
    }

    if (!this.question.details.variationRules) {
      this.question.details.variationRules = [];
    }

    this.question.details.variationRules.push({
      key: '',
      options: [],
      optionsStr: ''
    });
  }


  removeRule(index: number): void {
    if (!this.question.details.variationRules) {
      this.question.details.variationRules = [];
    }
    this.question.details.variationRules.splice(index, 1);
  }

  onSubmit(form: NgForm): void {
    if (form.invalid) return;
    if (!this.question.details.variationRules) {
      this.question.details.variationRules = [];
    }
    this.question.details.variationRules.forEach(rule => {
      rule.options =
        rule.optionsStr
          ?.split(',')
          .map(o => o.trim())
          .filter(Boolean) || [];
      delete (rule as any).optionsStr;
    });

    const action =
      this.question.details.id === EMPTY_ID
        ? this.questionService.create(this.question.details)
        : this.questionService.update(this.question.details.id, this.question.details);

    action.subscribe(() => this.saved.emit());
  }

  cancelForm(): void {
    this.cancel.emit();
  }
}
