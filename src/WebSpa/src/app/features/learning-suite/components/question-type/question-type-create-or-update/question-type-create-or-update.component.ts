import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { QuestionTypeService } from '../services/question-type.service';
import { IQuestionType } from '../interface/iQuestionType';

const EMPTY_ID = '00000000-0000-0000-0000-000000000000';

@Component({
  selector: 'app-question-type-create-or-update',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './question-type-create-or-update.component.html',
  styleUrl: './question-type-create-or-update.component.scss'
})
export class QuestionTypeCreateOrUpdateComponent implements OnInit {

  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  questionType: IQuestionType = this.getEmptyModel();

  isCodeFocused = false;
  isNameFocused = false;
  isDescriptionFocused = false;

  constructor(
    private questionTypeService: QuestionTypeService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const id = params['id'];
      if (!id || id === EMPTY_ID) return;

      this.questionTypeService.getById(id).subscribe({
        next: data => {
          this.questionType = data;
        }
      });
    });
  }

  getEmptyModel(): IQuestionType {
    return {
      id: EMPTY_ID,
      code: '',
      name: '',
      description: ''
    };
  }

  onSubmit(form: NgForm): void {
    if (form.invalid) return;

    const request =
      this.questionType.id === EMPTY_ID
        ? this.questionTypeService.create(this.questionType)
        : this.questionTypeService.update(this.questionType.id, this.questionType);

    request.subscribe(() => this.saved.emit());
  }

  cancelForm(): void {
    this.cancel.emit();
  }
}
