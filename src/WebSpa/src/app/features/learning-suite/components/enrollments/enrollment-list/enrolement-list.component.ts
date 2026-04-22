import { Component, OnInit } from '@angular/core';
import { IEnrollmentDetail } from '../interfaces/IEnrollmentDetail';
import { EnrollmentService } from '../services/enrollment.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IEnrollment } from '../interfaces/IEnrollment';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { EnrolementCreateOrEditComponent } from '../enrollment-create-or-edit/enrolement-create-or-edit.component';
import { ToastrService } from 'ngx-toastr';
import { CreateBulkEnrollmentsComponent } from '../create-bulk-enrollments/create-bulk-enrollments.component';

@Component({
  selector: 'app-enrolement-list',
  imports: [
    CommonModule,
    FormsModule,
    SideDrawerComponent,
    EnrolementCreateOrEditComponent,
    CreateBulkEnrollmentsComponent],
  templateUrl: './enrolement-list.component.html',
  styleUrl: './enrolement-list.component.scss'
})
export class EnrolementListComponent implements OnInit {
  enrolements: IEnrollmentDetail[] = [];
  filteredEnrolements: IEnrollmentDetail[] = [];
  pagedEnrolements: IEnrollmentDetail[] = [];
  searchText = '';
  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;
  drawerOpen = false;
  bulkDrawerOpen = false;
  selectedEnrolementId: string | null = null;
  expandedRows = new Set<string>();

  constructor(
    private enrollmentService: EnrollmentService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    const courseOfferingId = this.route.snapshot.paramMap.get('courseOfferingId');
    if (!courseOfferingId) return;
    this.getEnrolements(courseOfferingId);
  }

  getEnrolements(courseOfferingId: string) {
    this.enrollmentService.getEnrollments(courseOfferingId).subscribe({
      next: (data: IEnrollment) => {
        this.enrolements = data.detailsDtoList ?? [];
        this.applyFilter();
      }
    });
  }

  applyFilter() {
    const value = this.searchText.toLowerCase().trim();
    this.filteredEnrolements = this.enrolements.filter(e =>
      e.studentName?.toLowerCase().includes(value)
    );
    this.totalItems = this.filteredEnrolements.length;
    this.currentPage = 1;
    this.updatePage();
  }

  updatePage() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.pagedEnrolements = this.filteredEnrolements.slice(start, end);
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

  // Drawer methods
  openCreateDrawer() {
    this.selectedEnrolementId = null;
    this.drawerOpen = true;
    this.router.navigate([], {
      queryParams: { id: "00000000-0000-0000-0000-000000000000" },
      queryParamsHandling: 'merge'
    })
  }

  openBulkCreateDrawer() {
    
    this.selectedEnrolementId = null;
    this.bulkDrawerOpen = true;
    this.router.navigate([], {
      queryParams: { id: "bulk" },
      queryParamsHandling: 'merge'
    })
  }

  openEditDrawer(id: string) {
    this.selectedEnrolementId = id;
    this.drawerOpen = true;
    this.router.navigate([], {
      queryParams: { id: id },
      queryParamsHandling: 'merge'
    })
  }

  closeDrawer() {
    console.log("Closing drawer dd")
    this.drawerOpen = false;
    this.selectedEnrolementId = null;
    this.router.navigate([], {
      queryParams: { id: undefined },
      queryParamsHandling: 'merge'
    });
  }

    closeBulkDrawer() {
    console.log("Closing drawer dd")
    this.bulkDrawerOpen = false;
    this.selectedEnrolementId = null;
    this.router.navigate([], {
      queryParams: { id: undefined },
      queryParamsHandling: 'merge'
    });
  }

  onEnrolementSaved() {
    this.closeDrawer();
    const courseOfferingId = this.route.snapshot.paramMap.get('courseOfferingId');
    if (courseOfferingId) this.getEnrolements(courseOfferingId);
  }

   onEnrolementBulkSaved() {
    this.closeBulkDrawer();
    const courseOfferingId = this.route.snapshot.paramMap.get('courseOfferingId');
    if (courseOfferingId) this.getEnrolements(courseOfferingId);
  }

  // Expand/collapse child rows
  toggleRow(id: string) {
    if (this.expandedRows.has(id)) {
      this.expandedRows.delete(id);
    } else {
      this.expandedRows.add(id);
    }
  }

  isRowExpanded(id: string): boolean {
    return this.expandedRows.has(id);
  }

  goToAssessment(id: string) {
    // Navigate to assessment page or perform action
    console.log('Go to assessment for enrollment', id);
  }
}