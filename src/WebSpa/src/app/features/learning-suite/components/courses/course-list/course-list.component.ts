import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ICourse } from '../interfaces/ICourse';
import { CourseService } from '../services/course.service';
import { FormsModule } from '@angular/forms';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { CourseCreateOrUpdateComponent } from '../course-create-or-update/course-create-or-update.component';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule, FormsModule, SideDrawerComponent, CourseCreateOrUpdateComponent],
  templateUrl: './course-list.component.html',
  styleUrl: './course-list.component.scss'
})
export class CourseListComponent implements OnInit {

  courses: ICourse[] = [];
  filteredCourses: ICourse[] = [];
  pagedCourses: ICourse[] = [];

  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;

  sortColumn: keyof ICourse | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  searchText = '';
  drawerOpen = false;

  openCreateDrawer() {
    this.drawerOpen = true;
  }

  closeDrawer() {
    this.drawerOpen = false;
  }

  onCourseSaved() {
    this.closeDrawer();
    this.getCourses(); // refresh table
  }

  constructor(private courseService: CourseService) { }

  ngOnInit(): void {
    this.getCourses();
  }

  getCourses() {
    this.courseService.getCourses().subscribe({
      next: (data) => {
        this.courses = data;
        this.applyFilter();
      },
      error: (err) => console.error(err)
    });
  }

  applyFilter() {
    const value = this.searchText.toLowerCase().trim();

    this.filteredCourses = this.courses.filter(c =>
      c.courseCode.toLowerCase().includes(value) ||
      c.courseName.toLowerCase().includes(value)
    );

    this.totalItems = this.filteredCourses.length;
    this.currentPage = 1;

    this.applySort();
  }

  applySort(column?: keyof ICourse) {
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

      this.filteredCourses.sort((a, b) => {
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
    this.pagedCourses = this.filteredCourses.slice(start, end);
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
