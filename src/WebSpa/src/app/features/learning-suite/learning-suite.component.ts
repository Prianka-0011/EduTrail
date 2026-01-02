import { Component, HostListener, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule, isPlatformBrowser, NgIf } from '@angular/common';
import { NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MenuItem } from './interfaces/MenuItem';
import { filter } from 'rxjs';


@Component({
  selector: 'app-learning-suite',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    CommonModule,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatListModule,
    MatButtonModule,
    MatTooltipModule],
  templateUrl: './learning-suite.component.html',
  styleUrl: './learning-suite.component.scss'
})

export class LearningSuiteComponent {
  constructor(private router: Router) {
    this.router.events.pipe(filter(e => e instanceof NavigationEnd))
      .subscribe(() => {
        this.menu.forEach(item => {
          if (item.children) {
            item.children.forEach(child => {
              child.isActive = this.router.url.includes(child.route!);
            });
          }
        });
      });
  }
  isCollapsed = false;
  menu: MenuItem[] = [
    { label: 'Dashboard', icon: 'dashboard', route: 'dashboard' },
    {
      label: 'Courses', icon: 'book', isOpen: false, children: [
        { label: 'Course List', icon: 'chevron_right', route: 'courses' },
        { label: 'Create Course', icon: 'chevron_right', route: 'dashboard' }
      ]
    },
    { label: 'Assignments', icon: 'assignment', route: 'assignments' },
    { label: 'Settings', icon: 'settings', route: 'settings' }
  ];


  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
  }

 toggleSubmenu(item: MenuItem) {
  this.menu.forEach(i => {
    if (i !== item && i.children) i.isOpen = false;
  });
  item.isOpen = !item.isOpen;
}
}
