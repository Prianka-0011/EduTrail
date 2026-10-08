import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  Output,
  TemplateRef,
  ViewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface TableColumn {
  key: string;
  label: string;
  template?: TemplateRef<any>;
}

@Component({
  selector: 'app-reusable-data-table',
  standalone: true,
  templateUrl: './reusable-data-table.component.html',
  styleUrls: ['./reusable-data-table.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatSortModule,
    MatIconModule,
    MatButtonModule
  ]
})
export class ReusableTableComponent implements AfterViewInit {

  @Input() columns: TableColumn[] = [];

  @Input()
  set data(value: any[]) {
    this.dataSource.data = value || [];
  }

  @Input() expandedRows = new Set<string>();
  @Input() rowIdField = 'id';
  @Input() showExpand = false;
  @Input() showActions = true;
  @Input() emptyMessage = 'No records found';
  @Input() expandedTemplate!: TemplateRef<any>;

  @Output() expand = new EventEmitter<any>();
  @Output() edit = new EventEmitter<any>();
  @Output() rowClick = new EventEmitter<any>();

  @ViewChild(MatSort)
  sort!: MatSort;

  dataSource = new MatTableDataSource<any>([]);

  get displayedColumns(): string[] {
    return [
      ...(this.showExpand ? ['expand'] : []),
      ...this.columns.map(x => x.key),
      ...(this.showActions ? ['actions'] : [])
    ];
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  getRowId(row: any): string {
    return row[this.rowIdField];
  }

  isExpanded(row: any): boolean {
    return this.expandedRows.has(this.getRowId(row));
  }

  isExpansionRow = (index: number, row: any): boolean => {
    return this.isExpanded(row);
  };

  onExpand(row: any, event: Event): void {
    event.stopPropagation();
    this.expand.emit(row);
  }

  onEdit(row: any, event: Event): void {
    event.stopPropagation();
    this.edit.emit(row);
  }

  onRowClick(row: any): void {
    this.rowClick.emit(row);
  }
}