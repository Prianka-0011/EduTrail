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

  // The user's stored best score for this question (0-1), shown on the right.
  // null until an attempt has been scored.
  bestScore: number | null = null;
  // True briefly when the latest submission set a new personal best.
  newBest = false;

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
        this.bestScore = instance.bestScore ?? null;
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

        // Update the stored best. The backend keeps only the maximum, so a lower
        // attempt leaves bestScore unchanged; flag when this attempt is a new best.
        const serverBest = result.bestScore ?? result.score;
        this.newBest = this.bestScore === null || result.score > this.bestScore;
        this.bestScore = serverBest;
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
    this.newBest = false;
    this.error = '';
    this.correctAnswers = null;
    this.qs.createInstance(this.slug).subscribe({
      next: instance => {
        this.renderedHtml = '';
        setTimeout(() => {
          this.instanceId = instance.instanceId;
          this.renderedHtml = instance.renderedHtml;
          this.showCorrectAnswer = instance.showCorrectAnswer;
          this.bestScore = instance.bestScore ?? this.bestScore;
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
    // Return to wherever the question was opened from. Participants passes
    // ?from=participants so Back lands on the week/day tree, not the Browse list.
    const from = this.route.snapshot.queryParamMap.get('from');
    const target = from === 'participants'
      ? '/learning-suite/participants'
      : '/learning-suite/questions';
    this.router.navigate([target]);
  }

  get scorePercent(): number {
    return Math.round((this.result?.score ?? 0) * 100);
  }

  get bestScorePercent(): number {
    return Math.round((this.bestScore ?? 0) * 100);
  }

  get hasBestScore(): boolean {
    return this.bestScore !== null;
  }

  get passed(): boolean {
    return (this.result?.score ?? 0) >= 1;
  }

  objectKeys(obj: Record<string, unknown>): string[] {
    return Object.keys(obj);
  }

  // A code question may carry a model solution under the "solution" key,
  // shown as a code block; everything else renders as plain fields.
  get solutionCode(): string | null {
    const s = this.correctAnswers?.['solution'];
    return typeof s === 'string' ? s : null;
  }

  get fieldKeys(): string[] {
    if (!this.correctAnswers) return [];
    // Only render simple answer values. Never render object-shaped data (e.g. the
    // grader's {input, expected} test cases) even if it reaches the client — the
    // backend is the primary gate; this is defence in depth.
    return Object.keys(this.correctAnswers)
      .filter(k => k !== 'solution')
      .filter(k => this.isDisplayable(this.correctAnswers![k]));
  }

  private isDisplayable(v: unknown): boolean {
    if (v === null) return true;
    const t = typeof v;
    if (t === 'string' || t === 'number' || t === 'boolean') return true;
    if (Array.isArray(v)) return v.every(e => this.isDisplayable(e));
    return false; // plain objects (test-case shape) are never shown
  }

  get hasReveal(): boolean {
    return !!this.solutionCode || this.fieldKeys.length > 0;
  }

  formatAnswer(value: unknown): string {
    if (Array.isArray(value)) return value.join(', ');
    return String(value);
  }
}
