import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { UserService } from '../services/user.service';
import { IUser } from '../interfaces/iUser';
import { IUserDetail } from '../interfaces/iUserDetail';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';

type SortableUserField =
  | 'firstName'
  | 'lastName'
  | 'email'
  | 'isActive';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, SideDrawerComponent, FormsModule],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent implements OnInit {

  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  users: IUserDetail[] = [];
  filteredUsers: IUserDetail[] = [];
  pagedUsers: IUserDetail[] = [];

  selectedUserId: string | null = null;
  drawerOpen = false;

  searchText = '';

  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;

  sortColumn: SortableUserField | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.userService.getAll().subscribe(users => {
      this.users = users.flatMap(u => u.detailDtoList);
      this.applyFilter();
    });
  }

  openCreateDrawer() {
    this.selectedUserId = null;
    this.drawerOpen = true;

    this.router.navigate([], {
      queryParams: { id: '00000000-0000-0000-0000-000000000000' },
      queryParamsHandling: 'merge'
    });
  }

  openEditDrawer(id: string) {
    this.selectedUserId = id;
    this.drawerOpen = true;

    this.router.navigate([], {
      queryParams: { id },
      queryParamsHandling: 'merge'
    });
  }

  closeDrawer() {
    this.drawerOpen = false;
    this.selectedUserId = null;

    this.router.navigate([], {
      queryParams: { id: undefined },
      queryParamsHandling: 'merge'
    });
  }

  onUserSaved() {
    this.closeDrawer();
    this.getUsers();
  }

  applyFilter() {
    const value = this.searchText.toLowerCase().trim();

    this.filteredUsers = this.users.filter(u =>
      u.firstName?.toLowerCase().includes(value) ||
      u.lastName?.toLowerCase().includes(value) ||
      u.email?.toLowerCase().includes(value)
    );

    this.totalItems = this.filteredUsers.length;
    this.currentPage = 1;

    this.applySort();
  }

  applySort(column?: SortableUserField) {
    if (column) {
      if (this.sortColumn === column) {
        this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
      } else {
        this.sortColumn = column;
        this.sortDirection = 'asc';
      }
    }

    if (!this.sortColumn) {
      this.updatePage();
      return;
    }

    const key = this.sortColumn;

    this.filteredUsers.sort((a, b) => {
      const valueA = (a[key] ?? '').toString().toLowerCase();
      const valueB = (b[key] ?? '').toString().toLowerCase();

      return this.sortDirection === 'asc'
        ? valueA.localeCompare(valueB)
        : valueB.localeCompare(valueA);
    });

    this.updatePage();
  }

  updatePage() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.pagedUsers = this.filteredUsers.slice(start, end);
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
