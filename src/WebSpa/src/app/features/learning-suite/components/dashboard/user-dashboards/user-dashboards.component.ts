import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { ICourseOfferingByUserDetail } from '../interfaces/iCourseOfferingByUser';
import { UserDashboardService } from '../services/user-dashboard.service';

@Component({
  selector: 'app-user-dashboards',
  imports: [
    RouterOutlet,
    CommonModule,
    FormsModule,
    SideDrawerComponent,
  ],
  templateUrl: './user-dashboards.component.html',
  styleUrl: './user-dashboards.component.scss'
})
export class UserDashboardsComponent implements OnInit {
  ngOnInit(): void {
    console.log("I am at this ");
  }

  
}
