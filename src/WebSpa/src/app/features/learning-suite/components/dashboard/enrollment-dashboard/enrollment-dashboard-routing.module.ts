import { Routes } from "@angular/router";
import { EnrollmentProfileComponent } from "./components/enrollment-profile/enrollment-profile.component";

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
] 
