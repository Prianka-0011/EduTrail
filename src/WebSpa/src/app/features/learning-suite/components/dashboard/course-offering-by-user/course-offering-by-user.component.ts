import { CommonModule } from '@angular/common';
import {
  Component,
  OnInit,
  TemplateRef,
  ViewChild
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  ActivatedRoute,
  Router,
  RouterModule,
  RouterOutlet
} from '@angular/router';

import { UserDashboardService } from '../services/user-dashboard.service';
import { ICourseOfferingByUserDetail } from '../interfaces/ICourseOfferingByUser';

import { CustomCategory } from '../../../../../shared/interface/customCategory';

import {
  TableColumn
} from '../../../../../shared/components/reusable-data-table/reusable-data-table.component';

import {
  SideDrawerComponent
} from '../../../../../shared/components/side-drawer/side-drawer.component';

import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';


type SortColumn =
  | 'name'
  | 'year'
  | 'startDate'
  | 'endDate'
  | 'courseName'
  | 'instructorName'
  | 'termName';


@Component({
  selector: 'app-course-offering-by-user',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    RouterOutlet,
    SideDrawerComponent,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTableModule
  ],
  templateUrl: './course-offering-by-user.component.html',
  styleUrl: './course-offering-by-user.component.scss'
})
export class CourseOfferingByUserComponent implements OnInit {

  constructor(
    private courseOfferingService: UserDashboardService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

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

  sortColumn: SortColumn | '' = '';

  sortDirection: 'asc' | 'desc' = 'asc';

  expandedRows = new Set<string>();


  @ViewChild('courseTemplate', { static: true })
  courseTemplate!: TemplateRef<unknown>;

  @ViewChild('instructorTemplate', { static: true })
  instructorTemplate!: TemplateRef<unknown>;

  @ViewChild('termTemplate', { static: true })
  termTemplate!: TemplateRef<unknown>;

  @ViewChild('expandedTemplate', { static: true })
  expandedTemplate!: TemplateRef<unknown>;


  get columns(): TableColumn[] {
    return [
      {
        key: 'courseName',
        label: 'Course',
        template: this.courseTemplate
      },
      {
        key: 'instructorName',
        label: 'Instructor',
        template: this.instructorTemplate
      },
      {
        key: 'termName',
        label: 'Term',
        template: this.termTemplate
      }
    ];
  }


  get paged(): ICourseOfferingByUserDetail[] {
    return this.pagedCourseOfferings;
  }


  getTermClass(termType?: string | null): string {
    console.log(termType, "getTermClass")
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


  gotoEnrollementDashboard(courseOfferingId: string): void {
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


  getCourseOfferings(): void {
    this.courseOfferingService.getCourseOfferingByUser().subscribe({
      next: data => {
        console.log(data, 'data');

        this.courseOfferings = data?.detailsDtoList ?? [];

        this.applyFilter();
      },
      error: error => {
        console.error(
          'Error loading course offerings:',
          error
        );

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
      queryParams: {
        id
      },
      queryParamsHandling: 'merge'
    });
  }


  closeDrawer(): void {
    this.drawerOpen = false;

    this.selectedCourseOfferingId = null;

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        id: null
      },
      queryParamsHandling: 'merge'
    });
  }


  onCourseOfferingSaved(): void {
    this.closeDrawer();

    this.getCourseOfferings();
  }


  onCourseSaved(): void {
    this.onCourseOfferingSaved();
  }


  onTermSaved(): void {
    this.closeDrawer();

    this.getCourseOfferings();
  }


  applyFilter(): void {
    const value = this.searchText
      .toLowerCase()
      .trim();

    this.filteredCourseOfferings =
      this.courseOfferings.filter(item => {

        const courseName =
          item.courseName?.toLowerCase() ?? '';

        const instructorName =
          item.instructorName?.toLowerCase() ?? '';

        const termName =
          item.termName?.toLowerCase() ?? '';

        return (
          courseName.includes(value) ||
          instructorName.includes(value) ||
          termName.includes(value)
        );
      });

    this.totalItems =
      this.filteredCourseOfferings.length;

    this.currentPage = 1;

    this.applySort();
  }


  applySort(column?: SortColumn): void {

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


    if (this.sortColumn) {

      const columnToSort = this.sortColumn;

      this.filteredCourseOfferings.sort(
        (a, b) => {

          const valueA =
            this.getSortValue(a, columnToSort);

          const valueB =
            this.getSortValue(b, columnToSort);

          const result =
            valueA.localeCompare(
              valueB,
              undefined,
              {
                numeric: true,
                sensitivity: 'base'
              }
            );

          return this.sortDirection === 'asc'
            ? result
            : -result;
        }
      );
    }


    this.updatePage();
  }


  private getSortValue(
    item: ICourseOfferingByUserDetail,
    column: SortColumn
  ): string {

    switch (column) {

      case 'name':
      case 'courseName':
        return (
          item.courseName ?? ''
        ).toLowerCase();


      case 'year':
        return this.extractYear(
          item.termName
        );


      case 'startDate':
        return this.extractStartDate(
          item.termName
        );


      case 'endDate':
        return this.extractEndDate(
          item.termName
        );


      case 'instructorName':
        return (
          item.instructorName ?? ''
        ).toLowerCase();


      case 'termName':
        return (
          item.termName ?? ''
        ).toLowerCase();


      default:
        return '';
    }
  }


  private extractYear(
    termName?: string | null
  ): string {

    if (!termName) {
      return '';
    }

    const match =
      termName.match(/\b(19|20)\d{2}\b/);

    return match?.[0] ?? '';
  }


  private extractStartDate(
    termName?: string | null
  ): string {

    if (!termName) {
      return '';
    }

    return termName.toLowerCase();
  }


  private extractEndDate(
    termName?: string | null
  ): string {

    if (!termName) {
      return '';
    }

    return termName.toLowerCase();
  }


  updatePage(): void {

    const start =
      (this.currentPage - 1) *
      this.pageSize;

    const end =
      start + this.pageSize;

    this.pagedCourseOfferings =
      this.filteredCourseOfferings.slice(
        start,
        end
      );
  }


  changePageSize(size: number): void {

    this.pageSize = Number(size);

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

    if (!this.totalItems || !this.pageSize) {
      return 0;
    }

    return Math.ceil(
      this.totalItems / this.pageSize
    );
  }


  get rangeLabel(): string {

    if (!this.totalItems) {
      return '0 of 0';
    }

    const start =
      (this.currentPage - 1) *
      this.pageSize + 1;

    const end =
      Math.min(
        this.currentPage * this.pageSize,
        this.totalItems
      );

    return `${start} – ${end} of ${this.totalItems}`;
  }
}