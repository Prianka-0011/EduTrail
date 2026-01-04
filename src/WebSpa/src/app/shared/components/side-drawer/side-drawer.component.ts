import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter, TemplateRef } from '@angular/core';

@Component({
  selector: 'app-side-drawer',
  templateUrl: './side-drawer.component.html',
  imports: [CommonModule],
  styleUrls: ['./side-drawer.component.scss']
})
export class SideDrawerComponent {
  @Input() opened = false;
  @Input() width: string = '800px';
  @Input() title = '';
  @Input() headerTemplate?: TemplateRef<any>;
  @Output() closed = new EventEmitter<void>();

  close() {
    this.closed.emit();
  }
}
