import { Routes } from '@angular/router';
import { authRoutes } from './features/auth/auth-routing.module';
import { AuthComponent } from './features/auth/auth.component';
import { LearningSuiteComponent } from './features/learning-suite/learning-suite.component';
import { learningSuiteRoutes } from './features/learning-suite/learning-suite-routing.module';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'learning-suite',
        pathMatch: 'full'
    },
    {
        path: 'auth',
        component: AuthComponent,
        children: authRoutes
    },
    {
        path: 'learning-suite',
        component: LearningSuiteComponent,
        children: learningSuiteRoutes
    }, 
    {
        path: '**',
        redirectTo: 'learning-suite'
    }
];
