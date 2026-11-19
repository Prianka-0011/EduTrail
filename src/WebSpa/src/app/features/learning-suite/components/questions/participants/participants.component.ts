import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { QuestionCatalogService } from '../services/question-catalog.service';
import { IQuestionCatalogItem } from '../interfaces/IQuestionCatalogItem';
import { CommonService } from '../../../../../shared/services/common.service';
import { CustomCategory } from '../../../../../shared/interface/customCategory';

interface DayGroup {
  day: number | null;
  label: string;
  questions: IQuestionCatalogItem[];
  open: boolean;
}

interface WeekGroup {
  week: number | null;
  label: string;
  days: DayGroup[];
  open: boolean;
}

@Component({
  selector: 'app-participants',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './participants.component.html',
  styleUrls: ['./participants.component.scss']
})
export class ParticipantsComponent implements OnInit {
  weeks: WeekGroup[] = [];
  loading = true;
  error = '';
  isInstructor = false;

  // Per-question save state (keyed by slug) for the instructor editor.
  savingSlug: string | null = null;
  savedSlug: string | null = null;
  rowError: Record<string, string> = {};

  // Which week/day groups are expanded, remembered across re-grouping so the tree
  // doesn't collapse every time an instructor saves an assignment.
  private expandedWeeks = new Set<string>();
  private expandedDays = new Set<string>();
  private firstLoad = true;

  private items: IQuestionCatalogItem[] = [];

  constructor(
    private catalog: QuestionCatalogService,
    private commonService: CommonService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.commonService.getCurrentLoginUser().subscribe({
      next: user => {
        this.isInstructor = user.roles?.some(r => r.id === CustomCategory.RoleType.Instructor) ?? false;
      },
      error: () => { this.isInstructor = false; }
    });

    this.load();
  }

  private load(): void {
    this.catalog.getCatalog().subscribe({
      next: items => {
        this.items = items;
        this.weeks = this.group(items);
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.error = 'Failed to load questions. Make sure the backend is running.';
        this.loading = false;
      }
    });
  }

  private weekKey(week: number | null): string {
    return week === null ? 'null' : String(week);
  }

  private dayKey(week: number | null, day: number | null): string {
    return `${this.weekKey(week)}:${day === null ? 'null' : day}`;
  }

  // Build a Week -> Day -> Questions tree. Questions with no week/day fall into
  // an "Unassigned" week / "Unscheduled" day, always sorted last.
  private group(items: IQuestionCatalogItem[]): WeekGroup[] {
    const weekMap = new Map<number | null, Map<number | null, IQuestionCatalogItem[]>>();

    for (const q of items) {
      const w = q.week ?? null;
      const d = q.day ?? null;
      if (!weekMap.has(w)) weekMap.set(w, new Map());
      const dayMap = weekMap.get(w)!;
      if (!dayMap.has(d)) dayMap.set(d, []);
      dayMap.get(d)!.push(q);
    }

    // null (unassigned) sorts after any real number.
    const key = (v: number | null) => (v === null ? Number.POSITIVE_INFINITY : v);
    const sortedWeeks = [...weekMap.keys()].sort((a, b) => key(a) - key(b));

    // On the very first render, expand the first week and its days by default.
    if (this.firstLoad && sortedWeeks.length > 0) {
      const first = sortedWeeks[0];
      this.expandedWeeks.add(this.weekKey(first));
      for (const d of weekMap.get(first)!.keys()) {
        this.expandedDays.add(this.dayKey(first, d));
      }
      this.firstLoad = false;
    }

    return sortedWeeks.map(w => {
      const dayMap = weekMap.get(w)!;
      const sortedDays = [...dayMap.keys()].sort((a, b) => key(a) - key(b));

      const days: DayGroup[] = sortedDays.map(d => ({
        day: d,
        label: d === null ? 'Unscheduled' : `Day ${d}`,
        questions: dayMap.get(d)!.slice().sort((x, y) => x.title.localeCompare(y.title)),
        open: this.expandedDays.has(this.dayKey(w, d))
      }));

      return {
        week: w,
        label: w === null ? 'Unassigned' : `Week ${w}`,
        days,
        open: this.expandedWeeks.has(this.weekKey(w))
      };
    });
  }

  toggleWeek(w: WeekGroup): void {
    w.open = !w.open;
    const k = this.weekKey(w.week);
    if (w.open) this.expandedWeeks.add(k); else this.expandedWeeks.delete(k);
  }

  toggleDay(w: WeekGroup, d: DayGroup): void {
    d.open = !d.open;
    const k = this.dayKey(w.week, d.day);
    if (d.open) this.expandedDays.add(k); else this.expandedDays.delete(k);
  }

  open(q: IQuestionCatalogItem): void {
    // Tag the navigation so the player's "Back to Questions" returns here.
    this.router.navigate(['/learning-suite/questions', q.slug], {
      queryParams: { from: 'participants' }
    });
  }

  totalQuestions(w: WeekGroup): number {
    return w.days.reduce((n, d) => n + d.questions.length, 0);
  }

  // Normalise an input value (empty string / 0 / negative) to a valid week/day or null.
  private clean(v: number | null | undefined): number | null {
    if (v === null || v === undefined || (v as unknown as string) === '') return null;
    const n = Number(v);
    return Number.isFinite(n) && n >= 1 ? Math.floor(n) : null;
  }

  // Save a question's week/day assignment (instructor only).
  save(q: IQuestionCatalogItem): void {
    const week = this.clean(q.week);
    const day = this.clean(q.day);
    this.savingSlug = q.slug;
    this.savedSlug = null;
    delete this.rowError[q.slug];

    this.catalog.setSchedule(q.slug, week, day).subscribe({
      next: () => {
        this.savingSlug = null;
        this.savedSlug = q.slug;
        // Reflect the cleaned values, keep the stored item in sync, then re-group.
        q.week = week;
        q.day = day;
        const stored = this.items.find(i => i.slug === q.slug);
        if (stored) { stored.week = week; stored.day = day; }
        this.weeks = this.group(this.items);
        setTimeout(() => { if (this.savedSlug === q.slug) this.savedSlug = null; }, 2000);
      },
      error: err => {
        this.savingSlug = null;
        this.rowError[q.slug] = err?.status === 403
          ? 'Instructors only.'
          : err?.status === 400
            ? 'Week/Day must be 1 or more.'
            : 'Save failed.';
      }
    });
  }
}
