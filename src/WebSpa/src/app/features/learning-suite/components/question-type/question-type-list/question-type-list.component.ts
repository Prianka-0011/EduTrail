import { Component, OnInit } from '@angular/core';
import { IQuestionType } from '../interface/iQuestionType';
import { QuestionTypeService } from '../services/question-type.service';
import { Router } from '@angular/router';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { QuestionTypeCreateOrUpdateComponent } from '../question-type-create-or-update/question-type-create-or-update.component';

@Component({
  selector: 'app-question-type-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SideDrawerComponent,
    QuestionTypeCreateOrUpdateComponent
  ],
  templateUrl: './question-type-list.component.html',
  styleUrl: './question-type-list.component.scss'
})
export class QuestionTypeListComponent implements OnInit {
  types: IQuestionType[] = [];
  filtered: IQuestionType[] = [];
  paged: IQuestionType[] = [];

  selectedTypeId: string | null = null;
  drawerOpen = false;

  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;

  sortColumn: keyof IQuestionType | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  searchText = '';

  constructor(
    private questionTypeService: QuestionTypeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getAll();
  }

  getAll() {
    this.questionTypeService.getAll().subscribe({
      next: data => {
        this.types = data;
        this.applyFilter();
      },
      error: err => console.error(err)
    });
  }

  openCreateDrawer() {
    this.selectedTypeId = null;
    this.drawerOpen = true;
    this.router.navigate([], {
      queryParams: { id: '00000000-0000-0000-0000-000000000000' },
      queryParamsHandling: 'merge'
    });
  }

  openEditDrawer(id: string) {
    this.selectedTypeId = id;
    this.drawerOpen = true;
    this.router.navigate([], {
      queryParams: { id },
      queryParamsHandling: 'merge'
    });
  }

  closeDrawer() {
    this.drawerOpen = false;
    this.selectedTypeId = null;
    this.router.navigate([], {
      queryParams: { id: undefined },
      queryParamsHandling: 'merge'
    });
  }

  onQuestionTypeSaved() {
    this.closeDrawer();
    this.getAll();
  }

  applyFilter() {
    const value = this.searchText.toLowerCase().trim();

    this.filtered = this.types.filter(t =>
      t.code.toLowerCase().includes(value) ||
      (t.name ?? '').toLowerCase().includes(value)
    );

    this.totalItems = this.filtered.length;
    this.currentPage = 1;
    this.applySort();
  }

  applySort(column?: keyof IQuestionType) {
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
        const aVal = String(a[key] ?? '').toLowerCase();
        const bVal = String(b[key] ?? '').toLowerCase();
        return this.sortDirection === 'asc'
          ? aVal.localeCompare(bVal)
          : bVal.localeCompare(aVal);
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
    this.pageSize = +size;
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
