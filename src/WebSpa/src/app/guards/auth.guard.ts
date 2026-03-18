import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable, map, of } from 'rxjs';
import { AuthService } from '../features/auth/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router, private toaster:ToastrService) {}

  canActivate(): Observable<boolean> {
    return this.authService.isLoggedIn().pipe(
      map(IsAuthenticated => {
        if (!IsAuthenticated) {
          this.router.navigate(['/auth']);
          this.toaster.error("User not authenticated");
          return false;
        }
        return true;
      })
    );
  }
}
