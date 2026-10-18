import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { QuestionCatalogService } from '../services/question-catalog.service';
import { DynamicQuestionRendererDirective } from '../player/dynamic-question-renderer.directive';
import { COMPONENT_REGISTRY_PROVIDER } from '../player/component-registry';

const DEFAULT_HTML = `<pl-question-panel>
  <p>Your question text here. Use {{params.x}} for random values.</p>
</pl-question-panel>
<pl-number-input answers-name="result" label="Answer" atol="0"></pl-number-input>`;

const DEFAULT_PY = `import random

def generate(data):
    x = random.randint(1, 10)
    data['params']['x'] = x
    data['correct_answers']['result'] = x * 2  # set correct answer
`;

@Component({
  selector: 'app-question-add',
  standalone: true,
  imports: [CommonModule, FormsModule, DynamicQuestionRendererDirective],
  providers: [COMPONENT_REGISTRY_PROVIDER],
  templateUrl: './question-add.component.html',
  styleUrls: ['./question-add.component.scss'],
})
export class QuestionAddComponent {
  activeTab: 'form' | 'preview' = 'form';

  title = '';
  topic = '';
  tags = '';
  questionHtml = DEFAULT_HTML;
  serverPy = DEFAULT_PY;
  saving = false;
  error = '';

  constructor(private qs: QuestionCatalogService, private router: Router) {}

  save(): void {
    if (!this.title.trim()) { this.error = 'Title is required.'; return; }
    if (!this.questionHtml.trim()) { this.error = 'question.html is required.'; return; }
    if (!this.serverPy.trim()) { this.error = 'server.py is required.'; return; }

    this.saving = true;
    this.error = '';

    this.qs.createQuestion({
      title: this.title.trim(),
      topic: this.topic.trim(),
      tags: this.tags,
      questionHtml: this.questionHtml,
      serverPy: this.serverPy,
    }).subscribe({
      next: () => this.router.navigate(['/learning-suite/questions']),
      error: err => {
        this.error = err?.status === 403
          ? 'You do not have permission to add questions.'
          : 'Failed to save question. Make sure the backend is running.';
        this.saving = false;
      },
    });
  }

  goBack(): void {
    this.router.navigate(['/learning-suite/questions']);
  }
}
