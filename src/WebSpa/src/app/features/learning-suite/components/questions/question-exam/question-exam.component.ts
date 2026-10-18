import { Component, OnInit, OnDestroy, ViewChildren, QueryList, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';

import { QuestionCatalogService } from '../services/question-catalog.service';
import { DynamicQuestionRendererDirective } from '../player/dynamic-question-renderer.directive';
import { COMPONENT_REGISTRY_PROVIDER } from '../player/component-registry';
import { IQuestionCatalogItem } from '../interfaces/IQuestionCatalogItem';

interface ExamQuestion {
  slug: string;
  title: string;
  instanceId: string;
  renderedHtml: string;
}

type ExamPhase = 'loading' | 'exam' | 'submitting' | 'results';

const EXAM_SIZE = 5;
const EXAM_SECONDS = 600;

function formatTime(seconds: number): string {
  const m = Math.floor(seconds / 60);
  const s = seconds % 60;
  return `${String(m).padStart(2, '0')}:${String(s).padStart(2, '0')}`;
}

@Component({
  selector: 'app-question-exam',
  standalone: true,
  imports: [CommonModule, DynamicQuestionRendererDirective],
  providers: [COMPONENT_REGISTRY_PROVIDER],
  templateUrl: './question-exam.component.html',
  styleUrls: ['./question-exam.component.scss'],
})
export class QuestionExamComponent implements OnInit, OnDestroy {
  @ViewChildren(DynamicQuestionRendererDirective)
  renderers!: QueryList<DynamicQuestionRendererDirective>;

  private qs = inject(QuestionCatalogService);
  private router = inject(Router);

  readonly formatTime = formatTime;

  phase: ExamPhase = 'loading';
  loadError = false;
  currentIndex = 0;
  secondsLeft = EXAM_SECONDS;
  examQuestions: ExamQuestion[] = [];
  scores: number[] = [];
  showConfirm = false;
  unansweredCount = 0;

  private timerHandle: ReturnType<typeof setInterval> | null = null;

  ngOnInit(): void {
    this.setup();
  }

  setup(): void {
    this.loadError = false;
    this.phase = 'loading';
    this.qs.getCatalog().subscribe({
      next: questions => {
        const picked = this.pickRandom(questions, EXAM_SIZE);
        if (picked.length === 0) { this.loadError = true; return; }
        forkJoin(picked.map(q => this.qs.createInstance(q.slug))).subscribe({
          next: instances => {
            this.examQuestions = picked.map((q, i) => ({
              slug: q.slug,
              title: q.title,
              instanceId: instances[i].instanceId,
              renderedHtml: instances[i].renderedHtml,
            }));
            this.currentIndex = 0;
            this.secondsLeft = EXAM_SECONDS;
            this.showConfirm = false;
            this.scores = [];
            this.phase = 'exam';
            this.startTimer();
          },
          error: () => { this.loadError = true; },
        });
      },
      error: () => { this.loadError = true; },
    });
  }

  prev(): void {
    if (this.currentIndex > 0) {
      this.currentIndex--;
      this.showConfirm = false;
    }
  }

  next(): void {
    if (this.currentIndex < this.examQuestions.length - 1) {
      this.currentIndex++;
      this.showConfirm = false;
    }
  }

  onSubmitClick(): void {
    const unanswered = this.renderers.toArray()
      .filter(r => Object.keys(r.getAnswers()).length === 0).length;
    if (unanswered > 0) {
      this.unansweredCount = unanswered;
      this.showConfirm = true;
    } else {
      this.doSubmit();
    }
  }

  doSubmit(): void {
    this.stopTimer();
    this.showConfirm = false;
    this.phase = 'submitting';
    const allAnswers = this.renderers.toArray().map(r => r.getAnswers());
    forkJoin(
      this.examQuestions.map((q, i) => this.qs.submitAnswer(q.instanceId, allAnswers[i]))
    ).subscribe({
      next: results => {
        this.scores = results.map(r => r.score);
        this.phase = 'results';
      },
      error: () => {
        this.phase = 'exam';
        this.startTimer();
      },
    });
  }

  goHome(): void {
    this.router.navigate(['/learning-suite/dashboard']);
  }

  get correctCount(): number {
    return this.scores.filter(s => s >= 1).length;
  }

  get scorePercent(): number {
    return this.examQuestions.length
      ? Math.round((this.correctCount / this.examQuestions.length) * 100)
      : 0;
  }

  get passed(): boolean {
    return this.scorePercent >= 60;
  }

  ngOnDestroy(): void {
    this.stopTimer();
  }

  private startTimer(): void {
    this.stopTimer();
    this.timerHandle = setInterval(() => {
      this.secondsLeft--;
      if (this.secondsLeft <= 0) {
        this.stopTimer();
        this.doSubmit();
      }
    }, 1000);
  }

  private stopTimer(): void {
    if (this.timerHandle !== null) {
      clearInterval(this.timerHandle);
      this.timerHandle = null;
    }
  }

  private pickRandom<T>(arr: T[], n: number): T[] {
    return [...arr].sort(() => Math.random() - 0.5).slice(0, n);
  }
}
