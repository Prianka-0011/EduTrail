import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { IEnrollmentDetail } from '../interfaces/IEnrollmentDetail';
import { IEnrollment } from '../interfaces/IEnrollment';
import { EnrollmentService } from '../services/enrollment.service';

import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { EnrolementCreateOrEditComponent } from '../enrollment-create-or-edit/enrolement-create-or-edit.component';
import { CreateBulkEnrollmentsComponent } from '../create-bulk-enrollments/create-bulk-enrollments.component';

import { ToastrService } from 'ngx-toastr';

import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';


type SortableEnrollmentField =
  | 'studentName'
  | 'studentEmail'
  | 'enrolledDate';


@Component({
  selector: 'app-enrolement-list',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,

    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTableModule,

    SideDrawerComponent,
    EnrolementCreateOrEditComponent,
    CreateBulkEnrollmentsComponent
  ],

  templateUrl: './enrolement-list.component.html',
  styleUrl: './enrolement-list.component.scss'
})
export class EnrolementListComponent implements OnInit {

  constructor(
    private enrollmentService: EnrollmentService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}


  // =========================================================
  // DATA
  // =========================================================

  enrolements: IEnrollmentDetail[] = [];

  filteredEnrolements: IEnrollmentDetail[] = [];

  pagedEnrolements: IEnrollmentDetail[] = [];


  // =========================================================
  // SEARCH
  // =========================================================

  searchText = '';


  // =========================================================
  // PAGINATION
  // =========================================================

  pageSizeOptions = [5, 10, 20];

  pageSize = 10;

  currentPage = 1;

  totalItems = 0;


  // =========================================================
  // SORTING
  // =========================================================

  sortColumn: SortableEnrollmentField | '' = '';

  sortDirection: 'asc' | 'desc' = 'asc';


  // =========================================================
  // DRAWERS
  // =========================================================

  drawerOpen = false;

  bulkDrawerOpen = false;

  selectedEnrolementId: string | null = null;


  // =========================================================
  // EXPANDED ROWS
  // =========================================================

  expandedRows = new Set<string>();


  // =========================================================
  // INIT
  // =========================================================

  ngOnInit(): void {

    const courseOfferingId =
      this.route.snapshot.paramMap.get('courseOfferingId');

    if (!courseOfferingId) {
      return;
    }

    this.getEnrolements(courseOfferingId);
  }


  // =========================================================
  // GET ENROLLMENTS
  // =========================================================

  getEnrolements(courseOfferingId: string): void {

    this.enrollmentService
      .getEnrollments(courseOfferingId)
      .subscribe({

        next: (data: IEnrollment) => {

          this.enrolements =
            data.detailsDtoList ?? [];

          this.applyFilter();
        },

        error: (error) => {

          console.error(
            'Error loading enrollments:',
            error
          );

          this.enrolements = [];

          this.filteredEnrolements = [];

          this.pagedEnrolements = [];

          this.totalItems = 0;
        }

      });
  }


  // =========================================================
  // SEARCH / FILTER
  // =========================================================

  applyFilter(): void {

    const value =
      this.searchText
        .toLowerCase()
        .trim();


    this.filteredEnrolements =
      this.enrolements.filter(enrollment => {

        if (!value) {
          return true;
        }

        return (
          enrollment.studentName
            ?.toLowerCase()
            .includes(value) ||

          enrollment.studentEmail
            ?.toLowerCase()
            .includes(value) ||

          enrollment.enrolledDate
            ?.toString()
            .toLowerCase()
            .includes(value)
        );

      });


    this.totalItems =
      this.filteredEnrolements.length;


    this.currentPage = 1;


    this.applySort();
  }


  // =========================================================
  // SORT
  // =========================================================

  applySort(
    column?: SortableEnrollmentField
  ): void {

    if (column) {

      if (this.sortColumn === column) {

        this.sortDirection =
          this.sortDirection === 'asc'
            ? 'desc'
            : 'asc';

      } else {

        this.sortColumn = column;

        this.sortDirection = 'asc';
      }
    }


    if (!this.sortColumn) {

      this.updatePage();

      return;
    }


    const key =
      this.sortColumn;


    this.filteredEnrolements.sort(
      (a, b) => {

        const valueA =
          this.getSortValue(a, key);

        const valueB =
          this.getSortValue(b, key);


        const comparison =
          valueA.localeCompare(
            valueB,
            undefined,
            {
              numeric: true,
              sensitivity: 'base'
            }
          );


        return this.sortDirection === 'asc'
          ? comparison
          : -comparison;
      }
    );


    this.updatePage();
  }


  // =========================================================
  // SORT VALUE
  // =========================================================

  private getSortValue(
    enrollment: IEnrollmentDetail,
    key: SortableEnrollmentField
  ): string {

    const value =
      enrollment[key];


    if (value === null || value === undefined) {
      return '';
    }


    if (key === 'enrolledDate') {

      const date =
        new Date(value as any);


      if (!isNaN(date.getTime())) {

        return date
          .getTime()
          .toString();
      }
    }


    return value
      .toString()
      .toLowerCase();
  }


  // =========================================================
  // PAGINATION
  // =========================================================

  updatePage(): void {

    const start =
      (this.currentPage - 1)
      * this.pageSize;


    const end =
      start + this.pageSize;


    this.pagedEnrolements =
      this.filteredEnrolements.slice(
        start,
        end
      );
  }


  changePageSize(size: number | string): void {

    this.pageSize =
      Number(size);


    this.currentPage = 1;

    this.updatePage();
  }


  goToPage(page: number): void {

    if (
      page < 1 ||
      page > this.totalPages
    ) {
      return;
    }


    this.currentPage = page;

    this.updatePage();
  }


  get totalPages(): number {

    return Math.ceil(
      this.totalItems /
      this.pageSize
    );
  }


  get rangeLabel(): string {

    if (!this.totalItems) {
      return '0 of 0';
    }


    const start =
      (this.currentPage - 1)
      * this.pageSize + 1;


    const end =
      Math.min(
        this.currentPage * this.pageSize,
        this.totalItems
      );


    return `${start} – ${end} of ${this.totalItems}`;
  }


  // =========================================================
  // CREATE DRAWER
  // =========================================================

  openCreateDrawer(): void {

    this.selectedEnrolementId = null;

    this.drawerOpen = true;


    this.router.navigate([], {

      queryParams: {
        id: '00000000-0000-0000-0000-000000000000'
      },

      queryParamsHandling: 'merge'

    });
  }


  // =========================================================
  // BULK CREATE DRAWER
  // =========================================================

  openBulkCreateDrawer(): void {

    this.selectedEnrolementId = null;

    this.bulkDrawerOpen = true;


    this.router.navigate([], {

      queryParams: {
        id: 'bulk'
      },

      queryParamsHandling: 'merge'

    });
  }


  // =========================================================
  // EDIT DRAWER
  // =========================================================

  openEditDrawer(id: string): void {

    this.selectedEnrolementId = id;

    this.drawerOpen = true;


    this.router.navigate([], {

      queryParams: {
        id: id
      },

      queryParamsHandling: 'merge'

    });
  }


  // =========================================================
  // CLOSE CREATE / EDIT DRAWER
  // =========================================================

  closeDrawer(): void {

    this.drawerOpen = false;

    this.selectedEnrolementId = null;


    this.router.navigate([], {

      queryParams: {
        id: undefined
      },

      queryParamsHandling: 'merge'

    });
  }


  // =========================================================
  // CLOSE BULK DRAWER
  // =========================================================

  closeBulkDrawer(): void {

    this.bulkDrawerOpen = false;

    this.selectedEnrolementId = null;


    this.router.navigate([], {

      queryParams: {
        id: undefined
      },

      queryParamsHandling: 'merge'

    });
  }


  // =========================================================
  // SAVED
  // =========================================================

  onEnrolementSaved(): void {

    this.closeDrawer();


    const courseOfferingId =
      this.route.snapshot.paramMap.get(
        'courseOfferingId'
      );


    if (courseOfferingId) {

      this.getEnrolements(
        courseOfferingId
      );
    }
  }


  // =========================================================
  // BULK SAVED
  // =========================================================

  onEnrolementBulkSaved(): void {

    this.closeBulkDrawer();


    const courseOfferingId =
      this.route.snapshot.paramMap.get(
        'courseOfferingId'
      );


    if (courseOfferingId) {

      this.getEnrolements(
        courseOfferingId
      );
    }
  }


  // =========================================================
  // EXPAND / COLLAPSE
  // =========================================================

  toggleRow(id: string): void {

    if (this.expandedRows.has(id)) {

      this.expandedRows.delete(id);

    } else {

      this.expandedRows.add(id);
    }
  }


  isRowExpanded(id: string): boolean {

    return this.expandedRows.has(id);
  }


  // =========================================================
  // ASSESSMENT
  // =========================================================

  goToAssessment(id: string): void {

    console.log(
      'Go to assessment for enrollment:',
      id
    );
  }

}