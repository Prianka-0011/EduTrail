import { Routes } from "@angular/router";
import { SignInComponent } from "./sign-in/sign-in.component";

export const authRoutes: Routes = [
  {
    path: '',
    redirectTo: "login",
    pathMatch: "full"
  },
  {
    path: "login",
    component: SignInComponent
  }
] 
