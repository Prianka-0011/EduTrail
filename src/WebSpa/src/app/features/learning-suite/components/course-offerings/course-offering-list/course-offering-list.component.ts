import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterOutlet } from '@angular/router';

import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { CourseOfferingService } from '../services/course-offering.service';
import { ICourseOfferingDetail } from '../interfaces/ICourseOfferignDetail';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { CourseOfferingCreateOrEditComponent } from '../course-offering-create-or-edit/course-offering-create-or-edit.component';
import { MatTableModule } from '@angular/material/table';
import { CustomCategory } from '../../../../../shared/interface/customCategory';
@Component({
  selector: 'app-course-offering-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterOutlet,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    SideDrawerComponent,
    MatTableModule,
    CourseOfferingCreateOrEditComponent,
  ],
  templateUrl: './course-offering-list.component.html',
  styleUrl: './course-offering-list.component.scss'
})
export class CourseOfferingListComponent implements OnInit {
  constructor(
    private courseOfferingService: CourseOfferingService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  courseOfferings: ICourseOfferingDetail[] = [];
  filteredCourseOfferings: ICourseOfferingDetail[] = [];
  pagedCourseOfferings: ICourseOfferingDetail[] = [];
  selectedCourseOfferingId: string | null = null;
  drawerOpen = false;
  searchText = '';
  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;
  sortColumn: keyof ICourseOfferingDetail | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';
  expandedRows: { [id: string]: boolean } = {};

  toggleRow(id: string): void {
    this.expandedRows[id] = !this.expandedRows[id];
  }

  isRowExpanded(id: string): boolean {
    return !!this.expandedRows[id];
  }

  goToEnrollment(courseOfferingId: string): void {
    this.router.navigate([
      `/learning-suite/course-offerings/${courseOfferingId}/enrolement-list`
    ]);
  }

  getTermClass(termType?: string | null): string {
    switch (termType?.toLowerCase()) {
      case CustomCategory.TermType.Spring.toLowerCase():
        return 'term-spring';

      case CustomCategory.TermType.Fall.toLowerCase():
        return 'term-fall';

      case CustomCategory.TermType.Winter.toLowerCase():
        return 'term-winter';

      case CustomCategory.TermType.Summer.toLowerCase():
        return 'term-summer';

      default:
        return 'term-default';
    }
  }

  ngOnInit(): void {
    this.getCourseOfferings();
  }

  getCourseOfferings(): void {
    this.courseOfferingService.getCourseOfferings().subscribe({
      next: data => {
        this.courseOfferings = data.detailDtoList ?? [];
        this.applyFilter();
      },
      error: error => {
        this.courseOfferings = [];
        this.filteredCourseOfferings = [];
        this.pagedCourseOfferings = [];
        this.totalItems = 0;
      }
    });
  }

  openCreateDrawer(): void {
    this.selectedCourseOfferingId = null;
    this.drawerOpen = true;

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        id: '00000000-0000-0000-0000-000000000000'
      },
      queryParamsHandling: 'merge'
    });
  }

  openEditDrawer(id: string): void {
    this.selectedCourseOfferingId = id;
    this.drawerOpen = true;

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { id },
      queryParamsHandling: 'merge'
    });
  }

  closeDrawer(): void {
    this.drawerOpen = false;
    this.selectedCourseOfferingId = null;

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { id: undefined },
      queryParamsHandling: 'merge'
    });
  }

  onCourseOfferingSaved(): void {
    this.closeDrawer();
    this.getCourseOfferings();
  }

  applyFilter(): void {
    const value = this.searchText.toLowerCase().trim();

    this.filteredCourseOfferings = this.courseOfferings.filter(o =>
      o.courseName?.toLowerCase().includes(value) ||
      o.instructorName?.toLowerCase().includes(value) ||
      o.termName?.toLowerCase().includes(value)
    );

    this.totalItems = this.filteredCourseOfferings.length;
    this.currentPage = 1;
    this.applySort();
  }

  applySort(column?: keyof ICourseOfferingDetail): void {
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

  updatePage(): void {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;

    this.pagedCourseOfferings = this.filteredCourseOfferings.slice(start, end);
  }

  changePageSize(size: number): void {
    this.pageSize = Number(size);
    this.currentPage = 1;
    this.updatePage();
  }

  goToPage(page: number): void {
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