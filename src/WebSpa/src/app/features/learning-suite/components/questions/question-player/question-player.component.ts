import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { QuestionCatalogService } from '../services/question-catalog.service';
import { DynamicQuestionRendererDirective } from '../player/dynamic-question-renderer.directive';
import { COMPONENT_REGISTRY_PROVIDER } from '../player/component-registry';
import { SubmissionResult } from '../interfaces/IPlayer';

@Component({
  selector: 'app-question-player',
  standalone: true,
  imports: [CommonModule, DynamicQuestionRendererDirective],
  providers: [COMPONENT_REGISTRY_PROVIDER],
  templateUrl: './question-player.component.html',
  styleUrls: ['./question-player.component.scss'],
})
export class QuestionPlayerComponent implements OnInit {
  @ViewChild(DynamicQuestionRendererDirective) renderer!: DynamicQuestionRendererDirective;

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private qs = inject(QuestionCatalogService);

  slug = '';
  instanceId = '';
  renderedHtml = '';
  loading = true;
  submitting = false;
  shuffling = false;
  error = '';
  result: SubmissionResult | null = null;
  showCorrectAnswer = false;
  correctAnswers: Record<string, unknown> | null = null;
  loadingAnswers = false;

  ngOnInit(): void {
    this.slug = this.route.snapshot.paramMap.get('slug')!;
    this.loadInstance();
  }

  private loadInstance(): void {
    this.qs.createInstance(this.slug).subscribe({
      next: instance => {
        this.instanceId = instance.instanceId;
        this.renderedHtml = instance.renderedHtml;
        this.showCorrectAnswer = instance.showCorrectAnswer;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load question. Make sure the backend and Python service are running.';
        this.loading = false;
      },
    });
  }

  submit(): void {
    if (!this.renderer) return;
    const answers = this.renderer.getAnswers();
    this.submitting = true;
    this.result = null;

    this.qs.submitAnswer(this.instanceId, answers).subscribe({
      next: result => {
        this.result = result;
        this.submitting = false;
        this.renderer.setFeedback(result.feedback);
      },
      error: () => {
        this.error = 'Submission failed. Please try again.';
        this.submitting = false;
      },
    });
  }

  shuffle(): void {
    this.shuffling = true;
    this.result = null;
    this.error = '';
    this.correctAnswers = null;
    this.qs.createInstance(this.slug).subscribe({
      next: instance => {
        this.renderedHtml = '';
        setTimeout(() => {
          this.instanceId = instance.instanceId;
          this.renderedHtml = instance.renderedHtml;
          this.showCorrectAnswer = instance.showCorrectAnswer;
          this.shuffling = false;
        });
      },
      error: () => {
        this.error = 'Failed to generate a new variant. Please try again.';
        this.shuffling = false;
      },
    });
  }

  revealAnswers(): void {
    this.loadingAnswers = true;
    this.qs.getCorrectAnswers(this.instanceId).subscribe({
      next: answers => {
        this.correctAnswers = answers;
        this.loadingAnswers = false;
      },
      error: () => {
        this.error = 'Could not load correct answers.';
        this.loadingAnswers = false;
      },
    });
  }

  goBack(): void {
    this.router.navigate(['/learning-suite/questions']);
  }

  get scorePercent(): number {
    return Math.round((this.result?.score ?? 0) * 100);
  }

  get passed(): boolean {
    return (this.result?.score ?? 0) >= 1;
  }

  objectKeys(obj: Record<string, unknown>): string[] {
    return Object.keys(obj);
  }

  formatAnswer(value: unknown): string {
    if (Array.isArray(value)) return value.join(', ');
    return String(value);
  }
}
