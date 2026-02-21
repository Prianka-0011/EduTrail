import { Routes } from "@angular/router";
import { CourseListComponent } from "./components/courses/course-list/course-list.component";
import { DashboardComponent } from "./components/dashboard/dashboard/dashboard.component";
import { CourseCreateOrUpdateComponent } from "./components/courses/course-create-or-update/course-create-or-update.component";
import { UserDashboardComponent } from "./components/dashboard/user-dashboard/user-dashboard.component";
import { TermListComponent } from "./components/terms/term-list/term-list.component";
import { QuestionListComponent } from "./components/questions/question-list/question-list.component";
import { AssessmentListComponent } from "./components/assesments/assessment-list/assessment-list.component";
import { QuestionTypeListComponent } from "./components/question-type/question-type-list/question-type-list.component";
import { CourseOfferingListComponent } from "./components/course-offerings/course-offering-list/course-offering-list.component";
import { UserListComponent } from "./components/users/user-list/user-list.component";




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
    component: UserDashboardComponent,
    children: [
      {
        path: 'profile', // show user profile name and detail anong with role
        // component: UserDashboardComponent
      },
    ]
  },

  {
    path: 'questions',
    component: QuestionListComponent
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
  },
  {
    path: 'assesments',
    component: AssessmentListComponent
  },
  {
    path: 'question-types',
    component: QuestionTypeListComponent
  },
  {
    path: 'users',
    component: UserListComponent
  },
  {
    path: 'course-offerings',
    component: CourseOfferingListComponent,
    children: [
      {
        path: 'enrolement-list/:courseOfferingId' //Enrollment List component to be created
      },
      {
        path: 'lab-schedule/:courseOfferingId',
      }

    ]
  },
  {
    path: 'terms',
    component: TermListComponent
  }

] 
