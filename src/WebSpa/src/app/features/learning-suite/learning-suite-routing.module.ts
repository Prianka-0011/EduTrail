import { Routes } from "@angular/router";
import { CourseListComponent } from "./components/courses/course-list/course-list.component";
import { DashboardComponent } from "./components/dashboard/dashboard/dashboard.component";


export const learningSuiteRoutes : Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    component: DashboardComponent
  },
  {
    path: 'courses',
    component: CourseListComponent
  }
] 
