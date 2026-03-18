import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-reset-password-email',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './reset-password-email.component.html',
  styleUrl: './reset-password-email.component.scss'
})
export class ResetPasswordEmailComponent {

  constructor(private toastr: ToastrService, private authService: AuthService) {}

  email = '';
  isEmailFocused = false;
  isLoading = false;

  onSubmit() {
    if (!this.email) return;

   this.authService.resetEmailSend(this.email).subscribe({
    next: res=>{
      this.toastr.success("Reset Password email sent successfully")
    },
    error: err=> {
      this.toastr.error("Reset password email failed to send")
    }
   })
  }
}
