import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, RouterOutlet } from '@angular/router';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { UserDashboardService } from '../services/user-dashboard.service';
import { ToastrService } from 'ngx-toastr';
import { ICurrentLoginUserDetail } from '../interfaces/iCurrentLoginUserDetail';
import { MenuItem } from '../../../interfaces/MenuItem';
import { CustomCategory } from '../../../../../shared/interface/customCategory';

@Component({
  selector: 'app-enrollment-dashboard',
  imports: [
    RouterOutlet,
    RouterModule,
    CommonModule,
    FormsModule,
    SideDrawerComponent
  ],
  templateUrl: './enrollment-dashboard.component.html',
  styleUrls: ['./enrollment-dashboard.component.scss']
})
export class EnrollmentDashboardComponent implements OnInit {

  userDetail: ICurrentLoginUserDetail = {
    id: '',
    fullName: '',
    email: '',
    roles: []
  };

  menu: MenuItem[] = [];

  constructor(
    private service: UserDashboardService,
    private toast: ToastrService
  ) { }

  ngOnInit(): void {
    this.service.getCurrentLoginUser().subscribe({
      next: res => {
        this.userDetail = res;
        console.log("user role", this.userDetail)
        this.buildMenu();
      },
      error: () => {
        this.toast.error('Failed to load user data');
      }
    });
  }

  private buildMenu(): void {
    const roles = this.userDetail.roles?.map(c => c.id) || [];

    const allMenu: MenuItem[] = [
      {
        label: 'Personal',
        children: [
          {
            label: 'My Profile',
            icon: 'bi-person',
            route: 'enrollment-profile',
            rolesPermission: [
              CustomCategory.RoleType.Student,
              CustomCategory.RoleType.TA,
              CustomCategory.RoleType.Instructor
            ]
          }
        ]
      },
      {
        label: "Help",
        children: [
          {
            label: 'Schedule',
            icon: 'bi-calendar',
            route: 'ta-lab-schedule',
            rolesPermission: [
              CustomCategory.RoleType.TA,
              CustomCategory.RoleType.Student,
              CustomCategory.RoleType.Instructor
            ]
          },
          {
            label: 'Submit Help Request',
            icon: 'bi-question-circle',
            route: 'submit-help-request',
            rolesPermission: [CustomCategory.RoleType.Student]
          },
          {
            label: 'My Help Request List',
            icon: 'bi-list',
            route: 'my-help-request-list',
            rolesPermission: [
              CustomCategory.RoleType.Student,
            ]
          },
          {
            label: 'Help Request List',
            icon: 'bi-list',
            route: 'help-request-list',
            rolesPermission: [
              CustomCategory.RoleType.TA,
              CustomCategory.RoleType.Instructor
            ]
          },
        ]
      },
      {
        label: 'Admin',
        rolesPermission: [CustomCategory.RoleType.Instructor],
        children: [
          {
            label: 'Manage Users',
            icon: 'bi-gear',
            route: 'manage-users',
            rolesPermission: [CustomCategory.RoleType.Instructor]
          }
        ]
      }
    ];

    this.menu = this.filterMenuByRoles(allMenu, roles);
    console.log("menu", this.menu);
  }

  private filterMenuByRoles(menu: MenuItem[], userRoles: string[]): MenuItem[] {
    return menu
      .map(item => {
        let filteredChildren: MenuItem[] = [];
        if (item.children) {
          filteredChildren = this.filterMenuByRoles(item.children, userRoles);
        }

        const hasAccess = !item.rolesPermission || item.rolesPermission.some(r => userRoles.includes(r));

        if (hasAccess || filteredChildren.length > 0) {
          return { ...item, children: filteredChildren };
        }

        return null;
      })
      .filter(item => item !== null) as MenuItem[];
  }

}
