import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-sign-in',
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    RouterLinkActive,
  ],
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.scss'
})

export class SignInComponent {

  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router) { }

  user = { email: '', password: '' };
  isEmailFocused = false;
  isPasswordFocused = false;
  isLoading = false;
  errorMessage = '';
  showPassword: boolean = false;

  onSubmit() {
    if (!this.user.email || !this.user.password) {
      this.errorMessage = 'Email and password are required';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.signIn(this.user).subscribe({
      next: () => {
        this.isLoading = false;
        //this.toastr.success('Login successful', 'Success');
        this.router.navigate(['/learning-suite']);
      },
      error: () => {
        this.isLoading = false;
        this.toastr.error('Invalid email or password', 'Login Failed');
      }
    });

  }
}