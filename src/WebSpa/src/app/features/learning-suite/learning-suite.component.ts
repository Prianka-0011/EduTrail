
import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs';
import { MenuItem } from './interfaces/MenuItem';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-learning-suite',
  standalone: true,
  imports: [RouterOutlet,
    RouterLink,
    RouterLinkActive,
    CommonModule,],
  templateUrl: './learning-suite.component.html',
  styleUrl: './learning-suite.component.scss'
})
export class LearningSuiteComponent {

  isCollapsed = false;

  menu: MenuItem[] = [
    { label: 'Dashboard', icon: 'speedometer2', route: 'dashboard' },
    { label: 'User Dashboard', icon: 'bi bi-person-vcard', route: 'user-dashboard' },
    { label: 'Terms', icon: 'bi bi-person-vcard', route: 'terms' },
     { label: 'Questions', icon: 'bi bi-person-vcard', route: 'questions' },

    {
      label: 'Courses',
      icon: 'book',
      isOpen: false,
      children: [
        { label: 'Course List', icon: 'list', route: 'courses' },
        { label: 'Create Course', icon: 'plus-circle', route: 'courses/create' }
      ]
    },
    { label: 'Assignments', icon: 'clipboard-check', route: 'assignments' },
    { label: 'Settings', icon: 'gear', route: 'settings' }
  ];

  constructor(private router: Router) {
    this.router.events
      .pipe(filter(e => e instanceof NavigationEnd))
      .subscribe(() => this.syncActiveMenu());
  }

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
  }

  toggleSubmenu(item: MenuItem) {
    this.menu.forEach(i => {
      if (i !== item && i.children) i.isOpen = false;
    });
    item.isOpen = !item.isOpen;
  }

  private syncActiveMenu() {
    this.menu.forEach(item => {
      if (item.children) {
        item.isOpen = item.children.some(child =>
          this.router.url.includes(child.route!)
        );
      }
    });
  }
}