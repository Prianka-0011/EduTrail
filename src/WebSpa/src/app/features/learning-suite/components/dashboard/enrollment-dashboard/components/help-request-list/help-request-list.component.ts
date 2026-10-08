import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { IHelpRequest, IHelpRequestDetail } from '../../../interfaces/IHelpRequest';
import { UserDashboardService } from '../../../services/user-dashboard.service';
import { stringify } from 'node:querystring';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { LabRequestService } from '../../../services/lab-request.service';
import { IDropdownItem } from '../../../../../../../shared/interface/iDropdownItem';
import { ToastrService } from 'ngx-toastr';
import { HelpRequestDetailViewComponent } from '../help-request-detail-view/help-request-detail-view.component';
import { SideDrawerComponent } from '../../../../../../../shared/components/side-drawer/side-drawer.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';


@Component({
  selector: 'app-help-request-list',
  imports: [
    CommonModule,
    FormsModule,
    SideDrawerComponent,
    HelpRequestDetailViewComponent,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule
  ],
  templateUrl: './help-request-list.component.html',
  styleUrl: './help-request-list.component.scss'
})
export class HelpRequestListComponent implements OnInit {
  EMPTY_ID = '00000000-0000-0000-0000-000000000000';
  constructor(
    private labRequestService: LabRequestService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private toast: ToastrService
  ) { }
  drawerOpen = false;
  requests: IHelpRequestDetail[] = [];
  filtered: IHelpRequestDetail[] = [];
  paged: IHelpRequestDetail[] = [];
  statusList: IDropdownItem[] = [];

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
    const courseOfferingId = this.activeRoute.parent?.snapshot.paramMap.get('courseOfferingId') ?? this.EMPTY_ID;
    this.labRequestService.getAllLabRequest(courseOfferingId).subscribe({
      next: res => {
        console.log("res.detailsListDto", res.statusList)
        this.requests = res.detailsListDto || [];
        this.applyFilter();
        this.statusList = res.statusList ?? [];
      },
      error: err => console.error(err)
    });
  }

  saveStatus(request: IHelpRequestDetail) {
    const payload: IHelpRequest = {
      detailsDto: request
    };

    this.labRequestService.updateLabRequest(payload).subscribe({
      next: () => {
        this.getAllRequests()
        this.toast.success('Status updated successfully');
      },
      error: () => {
        this.toast.error('Failed to update status');
      }
    });
  }

  getStatusClass(status: string | null | undefined): string {
  if (!status) {
    return 'status-default';
  }

  const value = status.toLowerCase();

  if (
    value.includes('complete') ||
    value.includes('resolved') ||
    value.includes('approved') ||
    value.includes('closed')
  ) {
    return 'status-success';
  }

  if (
    value.includes('pending') ||
    value.includes('review') ||
    value.includes('waiting')
  ) {
    return 'status-warning';
  }

  if (
    value.includes('reject') ||
    value.includes('cancel') ||
    value.includes('failed')
  ) {
    return 'status-danger';
  }

  if (
    value.includes('progress') ||
    value.includes('assigned') ||
    value.includes('active')
  ) {
    return 'status-info';
  }

  return 'status-default';
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

  openDetailDrawer(id: string) {
    this.drawerOpen = true;
    this.router.navigate([], {
      queryParams: { id: id },
      queryParamsHandling: 'merge'
    })
  }

  closeDrawer() {
    this.drawerOpen = false;
    this.router.navigate([], {
      queryParams: { id: undefined },
      queryParamsHandling: 'merge'
    });
  }

}