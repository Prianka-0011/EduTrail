import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { IChanPass } from '../../interface/IChangePass';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-change-password',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss'
})
export class ChangePasswordComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private service: AuthService,
    private toaster: ToastrService,
    private router: Router
  ) { }

  token: string = "";
  newPassword: string = '';
  confirmPassword: string = '';

  showNewPassword: boolean = false;
  showConfirmPassword: boolean = false;

  isNewFocused = false;
  isConfirmFocused = false;
  isLoading: boolean = false;

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
    });
  }

  onSubmit() {
    if (!this.newPassword || !this.confirmPassword) return;

    if (this.newPassword !== this.confirmPassword) {
      this.toaster.error("Passwords do not match");
      return;
    }

    this.isLoading = true;

    const payload: IChanPass = {
      password: this.newPassword,
      token: this.token
    };

    console.log('Submitting:', payload);

    this.service.changePassword(payload).subscribe({
      next: (message) => {
        this.toaster.success(message);
        this.isLoading = false;
        this.router.navigate(['/auth']);
      },
      error: (err) => {
        this.toaster.error(err);
        this.isLoading = false;
      }
    });
  }
}