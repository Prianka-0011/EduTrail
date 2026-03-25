import { Routes } from "@angular/router";
import { EnrollmentProfileComponent } from "./components/enrollment-profile/enrollment-profile.component";
import { EnrollmentDashboardComponent } from "./enrollment-dashboard.component";
import { TaLabScheduleComponent } from "./components/ta-lab-schedule/ta-lab-schedule.component";
import { SubmitHelpRequestComponent } from "./components/submit-help-request/submit-help-request.component";
import { HelpRequestListComponent } from "./components/help-request-list/help-request-list.component";
import { CurrentUserHelpRequestListComponent } from "./components/current-user-help-request-list/current-user-help-request-list.component";


export const enrollmentRoutes: Routes = [


  {
    path: '',
    redirectTo: 'enrollment-profile',
    pathMatch: 'full'
  },
  {
    path: 'enrollment-profile',
    component: EnrollmentProfileComponent
  },
  {
    path: 'ta-lab-schedule',
    component: TaLabScheduleComponent
  },
  {
    path: 'submit-help-request',
    component: SubmitHelpRequestComponent
  },
  {
    path: 'help-request-list',
    component: HelpRequestListComponent
  },
  {
    path: 'my-help-request-list',
    component: CurrentUserHelpRequestListComponent
  }
];