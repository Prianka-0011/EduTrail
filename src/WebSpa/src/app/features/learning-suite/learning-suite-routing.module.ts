import { Routes } from "@angular/router";
import { CourseListComponent } from "./components/courses/course-list/course-list.component";
import { DashboardComponent } from "./components/dashboard/dashboard/dashboard.component";
import { CourseCreateOrUpdateComponent } from "./components/courses/course-create-or-update/course-create-or-update.component";
import { UserDashboardComponent } from "./components/dashboard/user-dashboard/user-dashboard.component";
import { TermListComponent } from "./components/terms/term-list/term-list.component";


export const learningSuiteRoutes: Routes = [
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
    path: 'user-dashboard',
    component: UserDashboardComponent
  },
  {
    path: 'terms',
    component: TermListComponent
  },
  {
    path: 'courses',
    component: CourseListComponent,
    children: [
      {
        path: 'create',
        component: CourseCreateOrUpdateComponent
      },
      {
        path: 'edit/:id',
        component: CourseCreateOrUpdateComponent
      }
    ]
  }
] 
