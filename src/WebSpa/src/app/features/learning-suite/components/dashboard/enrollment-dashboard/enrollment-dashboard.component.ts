import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';

@Component({
  selector: 'app-enrollment-dashboard',
   imports: [
    RouterOutlet,
    CommonModule,
    FormsModule,
    SideDrawerComponent,
  ],
  templateUrl: './enrollment-dashboard.component.html',
  styleUrl: './enrollment-dashboard.component.scss'
})
export class EnrollmentDashboardComponent {

}
