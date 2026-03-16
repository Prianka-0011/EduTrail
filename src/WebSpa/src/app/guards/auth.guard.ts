import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable, map, of } from 'rxjs';
import { AuthService } from '../features/auth/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(): Observable<any> {
    return this.authService.isLoggedIn().pipe(
      map(loggedIn => {
        if (!loggedIn) {
          this.router.navigate(['/auth']);
          return false;
        }
        else if (loggedIn) {
          this.router.navigate(['/learning-suite']);
         return true;
        }
        else {
          return false;
        }
      })
    );
  }
}
