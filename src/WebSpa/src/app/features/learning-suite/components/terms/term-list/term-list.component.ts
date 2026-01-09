import { Component, OnInit } from '@angular/core';
import { TermService } from '../services/term.service';
import { ITerm } from '../interfaces/iTerm';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TermCreateOrUppdateComponent } from '../term-create-or-uppdate/term-create-or-uppdate.component';



@Component({
  selector: 'app-term-list',
  imports: [CommonModule, FormsModule, SideDrawerComponent, TermCreateOrUppdateComponent],
  templateUrl: './term-list.component.html',
  styleUrl: './term-list.component.scss'
})
export class TermListComponent implements OnInit {

  constructor(private termService: TermService, private router : Router) { }
  terms: ITerm[] = [];
  filtered: ITerm[] = [];
  paged: ITerm[] = [];

  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;

  sortColumn: keyof ITerm | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  searchText = '';
  drawerOpen = false;
  selecteTermId: string | null = null;

  ngOnInit() {
    this.getAllTerms();
  }

   openCreateDrawer() {
    this.selecteTermId = null;
    this.drawerOpen = true;
    this.router.navigate([], {
      queryParams: { id: "00000000-0000-0000-0000-000000000000" },
      queryParamsHandling: 'merge'
    })
  }

  openEditDrawer(termId: string) {
    this.selecteTermId = termId;
    this.drawerOpen = true;
    this.router.navigate([], {
      queryParams: { id: termId },
      queryParamsHandling: 'merge'
    });
  }

  closeDrawer() {
    this.drawerOpen = false;
    this.selecteTermId = null;
    this.router.navigate([], {
      queryParams: { id: undefined },
      queryParamsHandling: 'merge'
    });
  }

  onTermSaved() {
    this.closeDrawer();
    this.getAllTerms();
  }
  getAllTerms() {
    this.termService.getAll().subscribe({
      next: (data) => {
        this.terms = data;
      }, error: (err) => {
        console.log(err)
      }
    })
  }

    // Filter Terms based on search text
  
    applyFilter() {
      const value = this.searchText.toLowerCase().trim();
  
      this.filtered = this.terms.filter(c =>
        c.name.toLowerCase().includes(value) 
        // ||
        // c.TermName.toLowerCase().includes(value)
      );
  
      this.totalItems = this.filtered.length;
      this.currentPage = 1;
  
      this.applySort();
    }
  
    applySort(column?: keyof ITerm) {
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
      this.paged = this.filtered.slice(start, end);
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
