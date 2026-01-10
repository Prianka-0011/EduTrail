import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { QuestionService } from '../services/question.service';

import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { IQuestion } from '../interfaces/iQuestion';
// import { QuestionCreateOrUpdateComponent } from '../question-create-or-update/question-create-or-update.component';

@Component({
  selector: 'app-question-list',
  standalone: true,
  imports: [CommonModule, FormsModule, SideDrawerComponent],
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.scss']
})
export class QuestionListComponent implements OnInit {
  questions: IQuestion[] = [];
  filtered: IQuestion[] = [];
  paged: IQuestion[] = [];

  selectedQuestionId: string | null = null;
  drawerOpen = false;

  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;

  sortColumn: keyof IQuestion | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  searchText = '';

  constructor(private questionService: QuestionService, private router: Router) {}

  ngOnInit(): void {
    this.getAllQuestions();
  }

  getAllQuestions() {
    this.questionService.getAll().subscribe({
      next: data => {
        this.questions = data;
        this.applyFilter();
      },
      error: err => console.error(err)
    });
  }

  openCreateDrawer() {
    this.selectedQuestionId = null;
    this.drawerOpen = true;
    this.router.navigate([], {
      queryParams: { id: '00000000-0000-0000-0000-000000000000' },
      queryParamsHandling: 'merge'
    });
  }

  openEditDrawer(questionId: string) {
    this.selectedQuestionId = questionId;
    this.drawerOpen = true;
    this.router.navigate([], {
      queryParams: { id: questionId },
      queryParamsHandling: 'merge'
    });
  }

  closeDrawer() {
    this.drawerOpen = false;
    this.selectedQuestionId = null;
    this.router.navigate([], {
      queryParams: { id: undefined },
      queryParamsHandling: 'merge'
    });
  }

  onQuestionSaved() {
    this.closeDrawer();
    this.getAllQuestions();
  }

  applyFilter() {
    const value = this.searchText.toLowerCase().trim();
    this.filtered = this.questions.filter(q => q.title.toLowerCase().includes(value));
    this.totalItems = this.filtered.length;
    this.currentPage = 1;
    this.applySort();
  }

  applySort(column?: keyof IQuestion) {
    if (column) {
      if (this.sortColumn === column) {
        this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
      } else {
        this.sortColumn = column;
        this.sortDirection = 'asc';
      }
    }

    if (this.sortColumn) {
      const key = this.sortColumn;
      this.filtered.sort((a, b) => {
        const valueA = String(a[key]).toLowerCase();
        const valueB = String(b[key]).toLowerCase();
        return this.sortDirection === 'asc'
          ? valueA.localeCompare(valueB)
          : valueB.localeCompare(valueA);
      });
    }

    this.updatePage();
  }

  updatePage() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.paged = this.filtered.slice(start, end);
  }

  changePageSize(size: number) {
    this.pageSize = size;
    this.currentPage = 1;
    this.updatePage();
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.updatePage();
  }

  get totalPages(): number {
    return Math.ceil(this.totalItems / this.pageSize);
  }

  get rangeLabel(): string {
    if (!this.totalItems) return '0 of 0';
    const start = (this.currentPage - 1) * this.pageSize + 1;
    const end = Math.min(this.currentPage * this.pageSize, this.totalItems);
    return `${start} – ${end} of ${this.totalItems}`;
  }
}
