import { Component, OnInit } from '@angular/core';
import { IAssessment } from '../interface/iAssessment';
import { Router } from '@angular/router';

import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { AssessmentService } from '../services/assessment.service';
import { CreateOrEditAssessmentComponent } from '../create-or-edit-assessment/create-or-edit-assessment.component';
// import { CreateOrEditAssessmentComponent } from '../create-or-edit-assessment/create-or-edit-assessment.component';


@Component({
  selector: 'app-assessment-list',
  imports: [CommonModule, FormsModule, SideDrawerComponent, CreateOrEditAssessmentComponent],
  templateUrl: './assessment-list.component.html',
  styleUrl: './assessment-list.component.scss'
})
export class AssessmentListComponent implements OnInit {
  constructor(private assessmentService: AssessmentService, private router: Router) { }
  assessments: IAssessment[] = [];
  selectedCourseId: string | null = null;
  filteredAssessments: IAssessment[] = [];
  pagedAssessments: IAssessment[] = [];

  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;

  sortColumn: keyof IAssessment | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  searchText = '';
  drawerOpen = false;


  openCreateDrawer() {
    this.selectedCourseId = null;
    this.drawerOpen = true;
    this.router.navigate([], {
      queryParams: { id: "00000000-0000-0000-0000-000000000000" },
      queryParamsHandling: 'merge'
    })
  }

  openEditDrawer(courseId: string) {
    this.selectedCourseId = courseId;
    this.drawerOpen = true;
    this.router.navigate([], {
      queryParams: { id: courseId },
      queryParamsHandling: 'merge'
    });
  }

  closeDrawer() {
    this.drawerOpen = false;
    this.selectedCourseId = null;
    this.router.navigate([], {
      queryParams: { id: undefined },
      queryParamsHandling: 'merge'
    });
  }

  onAssesmentSaved() {
    this.closeDrawer();
    this.getAssesments();
  }



  ngOnInit(): void {
    this.getAssesments();
  }

  getAssesments() {
    this.assessmentService.getAssessments().subscribe({
      next: (data) => {
        this.assessments = data;
        this.applyFilter();
      },
      error: (err) => console.error(err)
    });
  }

  // Filter courses based on search text

  applyFilter() {
    const value = this.searchText.toLowerCase().trim();

    this.filteredAssessments = this.assessments.filter(c =>
      c.title.toLowerCase().includes(value) ||
      c.description?.toLowerCase().includes(value)
    );

    this.totalItems = this.filteredAssessments.length;
    this.currentPage = 1;

    this.applySort();
  }

  applySort(column?: keyof IAssessment) {
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

      this.filteredAssessments.sort((a, b) => {
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
    this.pagedAssessments = this.filteredAssessments.slice(start, end);
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
