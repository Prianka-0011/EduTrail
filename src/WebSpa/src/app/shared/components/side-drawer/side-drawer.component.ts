import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-side-drawer',
  templateUrl: './side-drawer.component.html',
  styleUrls: ['./side-drawer.component.scss']
})
export class SideDrawerComponent {
  @Input() opened = false;
  @Input() title = '';
  @Output() closed = new EventEmitter<void>();

  close() {
    this.closed.emit();
  }
}
