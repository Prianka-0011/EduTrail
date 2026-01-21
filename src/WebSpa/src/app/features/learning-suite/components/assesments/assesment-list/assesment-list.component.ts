import { Component, OnInit } from '@angular/core';
import { IAssesment } from '../interface/iAssesment';
import { Router } from '@angular/router';
import { AssesmentService } from '../services/assesment.service';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CreateOrUpdateAssesmentComponent } from '../create-or-update-assesment/create-or-update-assesment.component';

@Component({
  selector: 'app-assesment-list',
  imports: [CommonModule, FormsModule, SideDrawerComponent, CreateOrUpdateAssesmentComponent],
  templateUrl: './assesment-list.component.html',
  styleUrl: './assesment-list.component.scss'
})
export class AssesmentListComponent implements OnInit {
  constructor(private assesmentService: AssesmentService, private router: Router) { }
  assesments: IAssesment[] = [];
  selectedCourseId: string | null = null;
  filteredAssesments: IAssesment[] = [];
  pagedAssesments: IAssesment[] = [];

  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;

  sortColumn: keyof IAssesment | '' = '';
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
    this.assesmentService.getAssesments().subscribe({
      next: (data) => {
        this.assesments = data;
        this.applyFilter();
      },
      error: (err) => console.error(err)
    });
  }

  // Filter courses based on search text

  applyFilter() {
    const value = this.searchText.toLowerCase().trim();

    this.filteredAssesments = this.assesments.filter(c =>
      c.title.toLowerCase().includes(value) ||
      c.description.toLowerCase().includes(value)
    );

    this.totalItems = this.filteredAssesments.length;
    this.currentPage = 1;

    this.applySort();
  }

  applySort(column?: keyof IAssesment) {
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

      this.filteredAssesments.sort((a, b) => {
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
    this.pagedAssesments = this.filteredAssesments.slice(start, end);
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
