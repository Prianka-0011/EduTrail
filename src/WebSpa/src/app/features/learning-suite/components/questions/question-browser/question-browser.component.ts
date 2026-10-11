import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

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

  constructor(private catalogService: QuestionCatalogService) {}

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

  // Clicking a card is intentionally a no-op for now; the question player
  // (render question.html + server.py) is a later step.
  open(_question: IQuestionCatalogItem): void {
    // TODO: navigate to the question player once it exists.
  }
}
