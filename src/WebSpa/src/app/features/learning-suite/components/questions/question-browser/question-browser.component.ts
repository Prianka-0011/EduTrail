import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { QuestionCatalogService } from '../services/question-catalog.service';
import { IQuestionCatalogItem } from '../interfaces/IQuestionCatalogItem';

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

  constructor(
    private catalogService: QuestionCatalogService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
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
}
