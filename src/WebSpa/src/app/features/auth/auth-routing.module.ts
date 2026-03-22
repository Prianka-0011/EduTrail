import { Routes } from "@angular/router";
import { SignInComponent } from "./components/sign-in/sign-in.component";
import { ResetPasswordEmailComponent } from "./components/reset-password-email/reset-password-email.component";

export const authRoutes: Routes = [
  {
    path: '',
    redirectTo: "login",
    pathMatch: "full"
  },
  {
    path: "login",
    component: SignInComponent
  },
  {
    path: "reset-password",
    component: ResetPasswordEmailComponent
  }
] 
