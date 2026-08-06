import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { SideDrawerComponent } from '../../../../../../../../shared/components/side-drawer/side-drawer.component';
import { PostService } from '../services/post.service';
import { IPostDetail } from '../interfaces/IPost';
import { NewPostComponent } from '../new-post/new-post.component';
import { StripHtmlPipe } from '../../../../../../../../shared/pipes/strip-html.pipe';
import { CustomCategory } from '../../../../../../../../shared/interface/customCategory';
import { MatIconModule } from '@angular/material/icon';
import { EditPostComponent } from '../edit-post/edit-post.component';
import { ViewPostComponent } from '../view-post/view-post.component';

@Component({
  selector: 'app-posts-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SideDrawerComponent,
    NewPostComponent,
    StripHtmlPipe,
    MatIconModule,
    EditPostComponent,
    ViewPostComponent
  ],
  templateUrl: './posts-list.component.html',
  styleUrl: './posts-list.component.scss'
})
export class PostsListComponent implements OnInit {
  constructor(
    private postService: PostService,
    private router: Router,
    private toast: ToastrService,
    private activeRoute: ActivatedRoute
  ) { }

  drawerOpen = false;
  viewDrawerOpen = false;

  EMPTY_ID = '00000000-0000-0000-0000-000000000000';
  currentPost = this.EMPTY_ID;

  posts: IPostDetail[] = [];
  filtered: IPostDetail[] = [];
  paged: IPostDetail[] = [];

  groupedPosts: {
    title: string;
    expanded: boolean;
    posts: IPostDetail[];
  }[] = [];

  pageSizeOptions = [5, 10, 20];
  pageSize = 10;
  currentPage = 1;
  totalItems = 0;
  courseOfferingId = "";
  sortColumn: keyof IPostDetail | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';
  searchText = '';

  ngOnInit(): void {
    this.courseOfferingId =
      this.activeRoute.parent?.snapshot.paramMap.get('courseOfferingId')
      ?? this.EMPTY_ID;

    this.getAllPosts(this.courseOfferingId);
  }

  getAllPosts(courseOfferingId: string): void {

    this.postService.getAllPosts(courseOfferingId)
      .subscribe({
        next: res => {

          this.posts = res.detailsListDto ?? [];

          this.applyFilter();
        },
        error: err => {
          console.error(err);
          this.toast.error('Failed to load posts.');
        }
      });
  }

  getPostTypeIcon(postTypeId?: string): string {
    switch (postTypeId) {
      case CustomCategory.PostTypes.Note:
        return 'note';

      case CustomCategory.PostTypes.Question:
        return 'help_outline';

      case CustomCategory.PostTypes.Poll:
        return 'poll';

      default:
        return 'article';
    }
  }

  applyFilter() {

    const value = this.searchText
      .toLowerCase()
      .trim();

    this.filtered = this.posts.filter(p =>

      (p.summary || '')
        .toLowerCase()
        .includes(value)

      ||

      (p.details || '')
        .toLowerCase()
        .includes(value)

      ||

      (p.postTypeName || '')
        .toLowerCase()
        .includes(value)
    );

    this.totalItems = this.filtered.length;

    this.currentPage = 1;

    this.applySort();

    this.groupPosts(this.filtered);
  }

  applySort(column?: keyof IPostDetail) {

    if (column) {

      if (this.sortColumn === column) {

        this.sortDirection =
          this.sortDirection === 'asc'
            ? 'desc'
            : 'asc';

      }
      else {
        this.sortColumn = column;
        this.sortDirection = 'asc';
      }
    }

    if (this.sortColumn) {
      const key = this.sortColumn;

      this.filtered.sort((a, b) => {
        const valueA =
          String(a[key] ?? '')
            .toLowerCase();

        const valueB =
          String(b[key] ?? '')
            .toLowerCase();

        return this.sortDirection === 'asc'
          ? valueA.localeCompare(valueB)
          : valueB.localeCompare(valueA);
      });
    }

    this.updatePage();
  }

  updatePage() {
    const start =
      (this.currentPage - 1) * this.pageSize;

    const end =
      start + this.pageSize;

    this.paged =
      this.filtered.slice(start, end);
  }

  groupPosts(posts: IPostDetail[]) {
    const groups: {
      [key: string]: {
        title: string;
        posts: IPostDetail[];
        expanded: boolean;
      }
    } = {};

    posts.forEach(post => {
      const date =
        new Date(post.createdDate ?? new Date());
      let title =
        date.toLocaleDateString(
          'en-US',
          {
            month: 'numeric',
            day: 'numeric',
            year: 'numeric'
          }
        );

      const today = new Date();
      const yesterday = new Date();

      yesterday.setDate(
        today.getDate() - 1
      );

      if (
        date.toDateString() ===
        today.toDateString()
      ) {
        title = 'Today';
      }
      else if (
        date.toDateString() ===
        yesterday.toDateString()
      ) {
        title = 'Yesterday';
      }

      if (!groups[title]) {

        groups[title] = {
          title,
          expanded: true,
          posts: []
        };

      }
      groups[title].posts.push(post);
    });

    this.groupedPosts =
      Object.values(groups);
  }

  changePageSize(size: number) {
    this.pageSize = +size;
    this.currentPage = 1;
    this.updatePage();
  }

  goToPage(page: number) {
    if (
      page < 1 ||
      page > this.totalPages
    ) {
      return;
    }
    this.currentPage = page;
    this.updatePage();
  }

  get totalPages(): number {
    return Math.ceil(
      this.totalItems / this.pageSize
    );
  }

  get rangeLabel(): string {
    if (!this.totalItems) {
      return '0 of 0';
    }

    const start =
      (this.currentPage - 1) *
      this.pageSize + 1;

    const end =
      Math.min(
        this.currentPage * this.pageSize,
        this.totalItems
      );

    return `${start} – ${end} of ${this.totalItems}`;
  }

  openDetailDrawer(id: string = this.EMPTY_ID): void {
    this.currentPost = id;
    this.drawerOpen = true;

    this.router.navigate([], {
      queryParams: { id },
      queryParamsHandling: 'merge'
    });
  }
  closeDrawer(): void {
    this.drawerOpen = false;
    this.currentPost = this.EMPTY_ID;

    this.getAllPosts(this.courseOfferingId);

    this.router.navigate([], {
      queryParams: {
        id: undefined
      },
      queryParamsHandling: 'merge'
    });
  }

  // deletePost(id: string) {


  //   if (
  //     !confirm(
  //       'Are you sure you want to delete this post?'
  //     )
  //   ) {

  //     return;

  //   }



  //   this.postService.deletePost(id)
  //     .subscribe({

  //       next: () => {

  //         this.toast.success(
  //           'Post deleted successfully.'
  //         );

  //         const courseOfferingId =
  //           this.activeRoute.parent?.snapshot.paramMap.get('courseOfferingId')
  //           ?? this.EMPTY_ID;
  //         this.getAllPosts(courseOfferingId);;

  //       },


  //       error: () => {

  //         this.toast.error(
  //           'Failed to delete post.'
  //         );

  //       }

  //     });


  // }


  openMenuId: string | null = null;

  toggleMenu(id: string) {

    this.openMenuId =
      this.openMenuId === id
        ? null
        : id;

  }

  viewPost(post: IPostDetail): void {
    this.viewDrawerOpen = true;
    const id = post.id;
    this.router.navigate([], {
      queryParams: { id },
      queryParamsHandling: 'merge'
    });
  }

  viewCloseDrawer(): void {
    this.viewDrawerOpen = false;
    this.getAllPosts(this.courseOfferingId);

    this.router.navigate([], {
      queryParams: {
        id: undefined
      },
      queryParamsHandling: 'merge'
    });
  }

  editPost(post: IPostDetail): void {
    this.currentPost = post.id;
    this.drawerOpen = true;

    this.router.navigate([], {
      queryParams: { id: post.id },
      queryParamsHandling: 'merge'
    });
  }

  archivePost(post: IPostDetail): void {

    if (
      !confirm(
        'Are you sure you want to archive this post?'
      )
    ) {
      return;
    }

    this.postService
      .archivePost(post.id)
      .subscribe({

        next: () => {

          this.toast.success(
            'Post archived successfully.'
          );

          this.getAllPosts(
            this.courseOfferingId
          );

        },

        error: () => {

          this.toast.error(
            'Failed to archive post.'
          );

        }

      });

  }

  pinPost(post: IPostDetail): void {

    const newStatus = !post.isPinned;

    this.postService
      .pinPost(post.id, newStatus)
      .subscribe({

        next: () => {

          // post.isPinned = newStatus;

          this.toast.success(
            newStatus
              ? 'Post pinned successfully.'
              : 'Post unpinned successfully.'
          );

        },

        error: () => {

          this.toast.error(
            'Failed to update pin status.'
          );

        }

      });

  }

  markAsUnread(post: IPostDetail): void {

    this.postService
      .markAsReadUnread(post.id, false)
      .subscribe({

        next: () => {

          // post.isRead = false;

          this.toast.success(
            'Post marked as unread.'
          );

        },

        error: () => {

          this.toast.error(
            'Failed to mark post unread.'
          );

        }

      });

  }

  deleteFromEveryone(post: IPostDetail): void {

    if (
      !confirm(
        'Are you sure you want to delete this post?'
      )
    ) {
      return;
    }


    this.postService
      .deletePost(post.id)
      .subscribe({

        next: () => {

          this.toast.success(
            'Post deleted successfully.'
          );


          this.getAllPosts(
            this.courseOfferingId
          );

        },


        error: () => {

          this.toast.error(
            'Failed to delete post.'
          );

        }

      });

  }

  favoritePost(post: IPostDetail): void {

    const status = !post.isFavorite;

    this.postService
      .favoritePost(post.id, status)
      .subscribe({

        next: () => {

          post.isFavorite = status;

          this.toast.success(
            status
              ? 'Added to favorites.'
              : 'Removed from favorites.'
          );

        },

        error: () => {

          this.toast.error(
            'Failed to update favorite.'
          );

        }

      });
  }

  toggleGroup(group: {
    title: string;
    expanded: boolean;
    posts: IPostDetail[];
  }): void {
    group.expanded = !group.expanded;
  }
}