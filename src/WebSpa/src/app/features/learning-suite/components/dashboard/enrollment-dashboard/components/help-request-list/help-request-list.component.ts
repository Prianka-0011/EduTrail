import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { IHelpRequestDetail } from '../../../interfaces/iHelpRequest';
import { UserDashboardService } from '../../../services/user-dashboard.service';

@Component({
  selector: 'app-help-request-list',
   imports: [CommonModule, FormsModule],
  templateUrl: './help-request-list.component.html',
  styleUrl: './help-request-list.component.scss'
})
export class HelpRequestListComponent implements OnInit {

  constructor(private labRequestService: UserDashboardService) {}

  requests: IHelpRequestDetail[] = [];
  filtered: IHelpRequestDetail[] = [];
  paged: IHelpRequestDetail[] = [];

  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;

  sortColumn: keyof IHelpRequestDetail | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  searchText = '';

  ngOnInit(): void {
    this.getAllRequests();
  }

  getAllRequests() {
    this.labRequestService.getAllLabRequest().subscribe({
      next: res => {
        this.requests = res.detailsListDto || [];
        this.applyFilter();
      },
      error: err => console.error(err)
    });
  }

  applyFilter() {

    const value = this.searchText.toLowerCase().trim();

    this.filtered = this.requests.filter(r =>
      (r.issueTitle || '').toLowerCase().includes(value) ||
      (r.studentName || '').toLowerCase().includes(value) ||
      (r.courseOfferingName || '').toLowerCase().includes(value)
    );

    this.totalItems = this.filtered.length;
    this.currentPage = 1;

    this.applySort();
  }

  applySort(column?: keyof IHelpRequestDetail) {

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

        const valueA = String(a[key] || '').toLowerCase();
        const valueB = String(b[key] || '').toLowerCase();

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