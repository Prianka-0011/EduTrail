import { CanActivate, Router } from "@angular/router";
import { map, Observable } from "rxjs";
import { AuthService } from "../features/auth/services/auth.service";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class GuestGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): Observable<boolean> {
    return this.authService.isLoggedIn().pipe(
      map(loggedIn => {
        if (loggedIn) {
          this.router.navigate(['/learning-suite']);
          return false;
        }
        return true;
      })
    );
  }
}
