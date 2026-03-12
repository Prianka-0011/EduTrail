import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-sign-in',
  imports: [CommonModule, FormsModule],
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.scss'
})
export class SignInComponent {
  user = { email: '', password: '' };
  isEmailFocused = false;
  isPasswordFocused = false;

  onSubmit() {
    if (this.user.email && this.user.password) {
      console.log('Form submitted', this.user);
    }
  }
}