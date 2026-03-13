import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-sign-in',
  imports: [CommonModule, FormsModule],
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.scss'
})

export class SignInComponent {

  constructor(private authService: AuthService) { }

  user = { email: '', password: '' };
  isEmailFocused = false;
  isPasswordFocused = false;
  isLoading = false;
  errorMessage = '';

  onSubmit() {
    if (!this.user.email || !this.user.password) {
      this.errorMessage = 'Email and password are required';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.signIn(this.user).subscribe({
      next: (token) => {
        localStorage.setItem('token', token);
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Invalid email or password';
        this.isLoading = false;
        console.error(err);
      }
    });
  }
}