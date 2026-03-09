import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule, RouterOutlet } from '@angular/router';
import { UserDashboardService } from '../services/user-dashboard.service';
import { ICourseOfferingByUserDetail } from '../interfaces/iCourseOfferingByUser';

@Component({
  selector: 'app-course-offering-by-user',
  imports: [
    RouterOutlet,
    CommonModule,
    FormsModule,
    RouterModule
  ],
  templateUrl: './course-offering-by-user.component.html',
  styleUrl: './course-offering-by-user.component.scss'
})
export class CourseOfferingByUserComponent implements OnInit {

  constructor(
    private courseOfferingService: UserDashboardService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  courseOfferings: ICourseOfferingByUserDetail[] = [];
  filteredCourseOfferings: ICourseOfferingByUserDetail[] = [];
  pagedCourseOfferings: ICourseOfferingByUserDetail[] = [];

  selectedCourseOfferingId: string | null = null;
  drawerOpen = false;

  searchText = '';

  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;

  sortColumn: keyof ICourseOfferingByUserDetail | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';
  expandedRows: { [id: string]: boolean } = {};

  // Collapsible child rows
  toggleRow(id: string) {
    this.expandedRows[id] = !this.expandedRows[id];
  }

  isRowExpanded(id: string): boolean {
    return !!this.expandedRows[id];
  }

  gotoEnrollementDashboard(courseOfferingId: string) {
    console.log('courseOfferingId', courseOfferingId);

    this.router.navigate([
      'learning-suite',
      'course-offering-by-user',
      courseOfferingId,
      'enrollement-dashboard'
    ]);
  }

  ngOnInit(): void {
    this.getCourseOfferings();
  }

  getCourseOfferings() {
    this.courseOfferingService.getCourseOfferingByUser().subscribe({
      next: data => {
        console.log('Received course offerings data:', data);
        this.courseOfferings = data.detailsDtoList ?? [];
        this.applyFilter();
      }
    });
  }

  openCreateDrawer() {
    this.selectedCourseOfferingId = null;
    this.drawerOpen = true;

    this.router.navigate([], {
      queryParams: { id: '00000000-0000-0000-0000-000000000000' },
      queryParamsHandling: 'merge'
    });
  }

  openEditDrawer(id: string) {
    this.selectedCourseOfferingId = id;
    this.drawerOpen = true;

    this.router.navigate([], {
      queryParams: { id },
      queryParamsHandling: 'merge'
    });
  }

  closeDrawer() {
    this.drawerOpen = false;
    this.selectedCourseOfferingId = null;

    this.router.navigate([], {
      queryParams: { id: undefined },
      queryParamsHandling: 'merge'
    });
  }

  onCourseOfferingSaved() {
    this.closeDrawer();
    this.getCourseOfferings();
  }

  applyFilter() {
    const value = this.searchText.toLowerCase().trim();
    console.log(this.courseOfferings, "this.courseOfferings")
    this.filteredCourseOfferings = this.courseOfferings.filter(o =>
      o.courseName?.toLowerCase().includes(value) ||
      o.instructorName?.toLowerCase().includes(value) ||
      o.termName?.toLowerCase().includes(value)
    );

    this.totalItems = this.filteredCourseOfferings.length;
    this.currentPage = 1;

    this.applySort();
  }

  applySort(column?: keyof ICourseOfferingByUserDetail) {
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

      this.filteredCourseOfferings.sort((a, b) => {
        const valueA = String(a[key] ?? '').toLowerCase();
        const valueB = String(b[key] ?? '').toLowerCase();

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
    this.pagedCourseOfferings = this.filteredCourseOfferings.slice(start, end);
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
