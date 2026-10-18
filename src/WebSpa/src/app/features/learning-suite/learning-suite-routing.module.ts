import { Routes } from "@angular/router";
import { CourseListComponent } from "./components/courses/course-list/course-list.component";

import { CourseCreateOrUpdateComponent } from "./components/courses/course-create-or-update/course-create-or-update.component";

import { TermListComponent } from "./components/terms/term-list/term-list.component";
import { QuestionBrowserComponent } from "./components/questions/question-browser/question-browser.component";
import { QuestionPlayerComponent } from "./components/questions/question-player/question-player.component";
import { QuestionAddComponent } from "./components/questions/question-add/question-add.component";
import { QuestionExamComponent } from "./components/questions/question-exam/question-exam.component";
import { AssessmentListComponent } from "./components/assesments/assessment-list/assessment-list.component";
import { QuestionTypeListComponent } from "./components/question-type/question-type-list/question-type-list.component";
import { CourseOfferingListComponent } from "./components/course-offerings/course-offering-list/course-offering-list.component";
import { UserListComponent } from "./components/users/user-list/user-list.component";
import { EnrolementListComponent } from "./components/enrollments/enrollment-list/enrolement-list.component";
import { CourseOfferingByUserComponent } from "./components/dashboard/course-offering-by-user/course-offering-by-user.component";
import { EnrollmentDashboardComponent } from "./components/dashboard/enrollment-dashboard/enrollment-dashboard.component";
import { enrollmentRoutes } from "./components/dashboard/enrollment-dashboard/enrollment-dashboard-routing.module";
import { LearningSuiteComponent } from "./learning-suite.component";
import { DashboardComponent } from "./components/dashboard/dashboard.component";


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
    path: 'course-offering-by-user',
    component: CourseOfferingByUserComponent,
  },
  {
    path: 'course-offering-by-user/:courseOfferingId/enrollement-dashboard',
    component: EnrollmentDashboardComponent,
    children: enrollmentRoutes
  },
  {
    path: 'questions',
    component: QuestionBrowserComponent
  },
  {
    path: 'questions/new',
    component: QuestionAddComponent
  },
  {
    path: 'exam',
    component: QuestionExamComponent
  },
  {
    path: 'questions/:slug',
    component: QuestionPlayerComponent
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
  },
  {
    path: 'course-offerings/:courseOfferingId/enrolement-list',
    component: EnrolementListComponent
  },
  {
    path: 'terms',
    component: TermListComponent
  }

] 
