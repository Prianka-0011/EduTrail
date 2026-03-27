import { Routes } from '@angular/router';
import { authRoutes } from './features/auth/auth-routing.module';
import { AuthComponent } from './features/auth/auth.component';
import { LearningSuiteComponent } from './features/learning-suite/learning-suite.component';
import { learningSuiteRoutes } from './features/learning-suite/learning-suite-routing.module';
import { AuthGuard } from './guards/auth.guard';
import { GuestGuard } from './guards/guest.guard';
import { ChangePasswordComponent } from './features/auth/components/change-password/change-password.component';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'auth',
        pathMatch: 'full'
    },
    {
        path: 'auth',
        component: AuthComponent,
        canActivate: [GuestGuard],
        children: authRoutes
    },
    {
        path: 'learning-suite',
        component: LearningSuiteComponent,
        canActivate: [AuthGuard],
        children: learningSuiteRoutes
    },
    {
        path: 'change-password-smtp-link',
        component: ChangePasswordComponent,
    },
    {
        path: '**',
        redirectTo: 'auth'
    }
];
