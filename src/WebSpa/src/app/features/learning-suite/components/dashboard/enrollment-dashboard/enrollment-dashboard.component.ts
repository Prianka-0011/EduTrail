import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule, RouterOutlet } from '@angular/router';
import { SideDrawerComponent } from '../../../../../shared/components/side-drawer/side-drawer.component';
import { UserDashboardService } from '../services/user-dashboard.service';
import { ToastrService } from 'ngx-toastr';
import { ICurrentLoginUserDetail } from '../interfaces/iCurrentLoginUserDetail';
import { MenuItem } from '../../../interfaces/MenuItem';
import { CustomCategory } from '../../../../../shared/interface/customCategory';
import { IEnrolementDetail } from '../../enrolements/interfaces/iEntolementDetail';
import { ChatComponent } from '../../../../../chat/components/chat/chat.component';
import * as signalR from '@microsoft/signalr';
import { enviroment } from '../../../../../../environments/environment';
import { Subscription } from 'rxjs';
import { ChatService } from '../../../../../chat/services/chat.service';

@Component({
  selector: 'app-enrollment-dashboard',
  imports: [
    RouterOutlet,
    RouterModule,
    CommonModule,
    FormsModule,
    SideDrawerComponent,
    ChatComponent
  ],
  templateUrl: './enrollment-dashboard.component.html',
  styleUrls: ['./enrollment-dashboard.component.scss']
})
export class EnrollmentDashboardComponent implements OnInit {
  activeUsers: IEnrolementDetail[] = [];
  isChatOpen = false;
  private hubConnection!: signalR.HubConnection;
  isTA = false;
  selectedChatUser: IEnrolementDetail = {
    id: '',
    courseOfferingId: '',
    userId: '',
    enrolledDate: '',
    isTa: false
  };
  showActiveUsers = true;
  courseOfferingId = "";
  userDetail: ICurrentLoginUserDetail = {
    id: '',
    fullName: '',
    email: '',
    roles: []
  };
  private messageSub!: Subscription;

  menu: MenuItem[] = [];

  constructor(
    private service: UserDashboardService,
    private toast: ToastrService,
    private route: ActivatedRoute,
    private chatService: ChatService
  ) { }

  ngOnInit(): void {
    this.courseOfferingId = this.route?.snapshot.paramMap.get('courseOfferingId') ?? '';
    this.messageSub = this.chatService.message$.subscribe(sender => {
      console.log('🔥 Subject message received in dashboard:', sender);
      this.handleIncomingMessage(sender);
    });
    this.service.getCurrentLoginUser().subscribe({
      next: res => {
        this.userDetail = res;
        this.isTA = this.userDetail.roles?.some(
          r => r.id === CustomCategory.RoleType.TA
        ) ?? false;
        this.buildMenu();
      },
      error: () => this.toast.error('Failed to load user data')
    });

    this.loadActiveUsers();
  }

  private handleIncomingMessage(sender: IEnrolementDetail): void {
    let matchedUser = this.activeUsers.find(u => u.userId === sender.userId);

    if (!matchedUser) {
      matchedUser = {
        ...sender,
        studentName: sender.studentName || 'Unknown User'
      };
    }

    const wasChatOpen = this.isChatOpen;

    this.selectedChatUser = { ...matchedUser };

    if (!wasChatOpen) {
      this.isChatOpen = true;
    }

    this.showActiveUsers = true;

    this.toast.info(`New message from ${matchedUser.studentName}`);
  }

  ngOnDestroy(): void {
    this.messageSub?.unsubscribe();
  }


  toggleActiveUsers() {
    this.showActiveUsers = !this.showActiveUsers;
  }

  closeChat(): void {
    this.selectedChatUser = null as any;
    this.isChatOpen = false;
  }




  private loadActiveUsers(): void {
    this.courseOfferingId = this.route?.snapshot.paramMap.get('courseOfferingId') ?? '';
    if (!this.courseOfferingId) return;

    this.service.loadActiveUsers(this.courseOfferingId).subscribe({
      next: users => {
        this.activeUsers = users.detailsDtoList ?? [];
      },
      error: () => console.log('Failed to load active users')
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
        label: 'Help',
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
            rolesPermission: [CustomCategory.RoleType.Student]
          },
          {
            label: 'Help Request List',
            icon: 'bi-list',
            route: 'help-request-list',
            rolesPermission: [
              CustomCategory.RoleType.TA,
              CustomCategory.RoleType.Instructor
            ]
          }
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
          },
          {
            label: 'Help Request Dashboard',
            icon: 'bi-gear',
            route: 'help-request-dashboard',
            rolesPermission: [CustomCategory.RoleType.Instructor, CustomCategory.RoleType.TA]
          }
        ]
      }
    ];

    this.menu = this.filterMenuByRoles(allMenu, roles);
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

  openChat(user: IEnrolementDetail) {
    this.isChatOpen = false;

    setTimeout(() => {
      this.selectedChatUser = { ...user };
      this.isChatOpen = true;
    });
  }

}
