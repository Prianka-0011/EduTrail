import { Routes } from "@angular/router";
import { EnrollmentProfileComponent } from "./components/enrollment-profile/enrollment-profile.component";
import { EnrollmentDashboardComponent } from "./enrollment-dashboard.component";
import { TaLabScheduleComponent } from "./components/ta-lab-schedule/ta-lab-schedule.component";


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
  }

];