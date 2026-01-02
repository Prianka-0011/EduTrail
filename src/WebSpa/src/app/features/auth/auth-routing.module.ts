import { Routes } from "@angular/router";

export const authRoutes: Routes = [
  {
    path: '',
    redirectTo: "auth/login",
    pathMatch: "full"
  }
] 
