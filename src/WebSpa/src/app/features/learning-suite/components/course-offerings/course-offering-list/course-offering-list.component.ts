import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { CourseOfferingService } from '../services/course-offering.service';
import { ICourseOffering } from '../interfaces/iCourseOffering';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';


@Component({
  selector: 'app-course-offering-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SideDrawerComponent,
    // CourseOfferingCreateOrUpdateComponent
  ],
  templateUrl: './course-offering-list.component.html',
  styleUrl: './course-offering-list.component.scss'
})
export class CourseOfferingListComponent implements OnInit {

  constructor(
    private courseOfferingService: CourseOfferingService,
    private router: Router
  ) {}

  courseOfferings: ICourseOffering[] = [];
  filteredCourseOfferings: ICourseOffering[] = [];
  pagedCourseOfferings: ICourseOffering[] = [];

  selectedCourseOfferingId: string | null = null;
  drawerOpen = false;

  searchText = '';

  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;

  sortColumn: keyof ICourseOffering | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  ngOnInit(): void {
    this.getCourseOfferings();
  }

  getCourseOfferings() {
    this.courseOfferingService.getCourses().subscribe({
      next: data => {
        this.courseOfferings = data;
        this.applyFilter();
      },
      error: err => console.error(err)
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

    this.filteredCourseOfferings = this.courseOfferings.filter(o =>
      o.courseName?.toLowerCase().includes(value) ||
      o.instructorName?.toLowerCase().includes(value) ||
      o.termName?.toLowerCase().includes(value)
    );

    this.totalItems = this.filteredCourseOfferings.length;
    this.currentPage = 1;

    this.applySort();
  }

  applySort(column?: keyof ICourseOffering) {
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
