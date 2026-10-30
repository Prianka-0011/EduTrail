import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { QuestionCatalogService } from '../services/question-catalog.service';
import { IQuestionCatalogItem } from '../interfaces/IQuestionCatalogItem';
import { CommonService } from '../../../../../shared/services/common.service';
import { CustomCategory } from '../../../../../shared/interface/customCategory';

@Component({
  selector: 'app-question-browser',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './question-browser.component.html',
  styleUrls: ['./question-browser.component.scss']
})
export class QuestionBrowserComponent implements OnInit {
  questions: IQuestionCatalogItem[] = [];
  loading = true;
  error = '';
  isInstructor = false;

  constructor(
    private catalogService: QuestionCatalogService,
    private commonService: CommonService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.commonService.getCurrentLoginUser().subscribe({
      next: user => {
        this.isInstructor = user.roles?.some(r => r.id === CustomCategory.RoleType.Instructor) ?? false;
      },
      error: () => { this.isInstructor = false; }
    });

    this.loadQuestions();
  }

  private loadQuestions(): void {
    this.catalogService.getCatalog().subscribe({
      next: items => {
        this.questions = items;
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.error = 'Failed to load questions. Make sure the backend is running.';
        this.loading = false;
      }
    });
  }

  // Open the interactive player for a question (learning-suite/questions/:slug).
  open(question: IQuestionCatalogItem): void {
    this.router.navigate([question.slug], { relativeTo: this.route });
  }

  // Delete a question (instructors only). Stops the click from opening the player.
  deleteQuestion(question: IQuestionCatalogItem, event: MouseEvent): void {
    event.stopPropagation();
    if (!confirm(`Delete "${question.title}"? This permanently removes the question and cannot be undone.`)) return;

    this.catalogService.deleteQuestion(question.slug).subscribe({
      next: () => {
        this.questions = this.questions.filter(q => q.slug !== question.slug);
      },
      error: err => {
        alert(err?.status === 403
          ? 'You do not have permission to delete questions.'
          : 'Failed to delete question.');
      }
    });
  }
}
