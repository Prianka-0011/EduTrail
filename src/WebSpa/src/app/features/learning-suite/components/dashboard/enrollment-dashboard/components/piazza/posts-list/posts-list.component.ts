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
import { MatTooltipModule } from '@angular/material/tooltip';

imports: [
  CommonModule,
  FormsModule,
  SideDrawerComponent,
  NewPostComponent,
  StripHtmlPipe,
  MatIconModule,
  MatTooltipModule,
  EditPostComponent,
  ViewPostComponent
]
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
    MatTooltipModule,
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
      this.activeRoute.parent?.snapshot.paramMap.get(
        'courseOfferingId'
      ) ?? this.EMPTY_ID;

    this.getAllPosts(
      this.courseOfferingId
    );

  }

  getAllPosts(courseOfferingId: string): void {

    this.postService
      .getAllPosts(courseOfferingId)
      .subscribe({

        next: (res) => {

          this.posts =
            res.detailsListDto ?? [];

          this.applyFilter();

        },

        error: () => {

          this.toast.error(
            'Failed to load posts.'
          );

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

  applyFilter(): void {

    const value =
      this.searchText
        .toLowerCase()
        .trim();

    this.filtered =
      this.posts.filter(post =>

        (post.summary ?? '')
          .toLowerCase()
          .includes(value)

        ||

        (post.details ?? '')
          .toLowerCase()
          .includes(value)

        ||

        (post.postTypeName ?? '')
          .toLowerCase()
          .includes(value)

      );

    this.totalItems =
      this.filtered.length;

    this.currentPage = 1;

    this.applySort();

    this.groupPosts(
      this.filtered
    );

  }

  applySort(column?: keyof IPostDetail): void {

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

      const key =
        this.sortColumn;

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

  updatePage(): void {

    const start =
      (this.currentPage - 1)
      * this.pageSize;

    this.paged =
      this.filtered.slice(
        start,
        start + this.pageSize
      );

  }

  groupPosts(posts: IPostDetail[]): void {

    const now = new Date();

    const startOfWeek = new Date(now);
    startOfWeek.setDate(now.getDate() - now.getDay());


    const formatDate = (date: Date): string => {
      return date.toLocaleDateString('en-US', {
        month: 'numeric',
        day: 'numeric',
        year: 'numeric'
      });
    };


    const getWeekRange = (week: number) => {

      const start = new Date(startOfWeek);

      start.setDate(
        startOfWeek.getDate() - (week - 1) * 7
      );


      const end = new Date(start);

      end.setDate(
        start.getDate() + 6
      );


      return { start, end };

    };


    const pinned = posts.filter(
      x => x.isPinned
    );


    const favorite = posts.filter(
      x => x.isFavorite && !x.isPinned
    );


    const normalPosts = posts.filter(
      x => !x.isPinned && !x.isFavorite
    );


    const groups: {
      title: string;
      expanded: boolean;
      posts: IPostDetail[];
    }[] = [];



    if (pinned.length) {

      groups.push({
        title: '📌 Pinned Posts',
        expanded: true,
        posts: pinned
      });

    }



    if (favorite.length) {

      groups.push({
        title: '⭐ Favorite Posts',
        expanded: true,
        posts: favorite
      });

    }



    for (let i = 1; i <= 4; i++) {

      const range = getWeekRange(i);


      const weekPosts = normalPosts.filter(post => {

        const date = new Date(post.createdDate ?? '');

        return date >= range.start &&
          date <= range.end;

      });



      if (weekPosts.length) {

        groups.push({

          title: `📅 Week ${i} (${formatDate(range.start)} - ${formatDate(range.end)})`,

          expanded: true,

          posts: weekPosts

        });

      }

    }



    const week4Range = getWeekRange(4);


    const olderPosts = normalPosts.filter(post => {

      const date = new Date(post.createdDate ?? '');

      return date < week4Range.start;

    });



    if (olderPosts.length) {

      groups.push({

        title: `📁 Older Posts (Before ${formatDate(week4Range.start)})`,

        expanded: true,

        posts: olderPosts

      });

    }


    this.groupedPosts = groups;

  }

  openDetailDrawer(
    id: string = this.EMPTY_ID
  ): void {

    this.currentPost = id;
    this.drawerOpen = true;

    this.router.navigate([], {

      queryParams: {
        id
      },

      queryParamsHandling: 'merge'

    });

  }

  closeDrawer(): void {

    this.drawerOpen = false;
    this.currentPost = this.EMPTY_ID;

    this.getAllPosts(
      this.courseOfferingId
    );

    this.router.navigate([], {

      queryParams: {
        id: undefined
      },

      queryParamsHandling: 'merge'

    });

  }

  viewPost(post: IPostDetail): void {

    this.viewDrawerOpen = true;

    this.router.navigate([], {

      queryParams: {
        id: post.id
      },

      queryParamsHandling: 'merge'

    });

  }

  viewCloseDrawer(): void {

    this.viewDrawerOpen = false;

    this.getAllPosts(
      this.courseOfferingId
    );

  }

  editPost(post: IPostDetail): void {

    this.currentPost =
      post.id;

    this.drawerOpen = true;

    this.router.navigate([], {

      queryParams: {
        id: post.id
      },

      queryParamsHandling: 'merge'

    });

  }

  pinPost(post: IPostDetail): void {

    const status =
      !post.isPinned;

    this.postService
      .pinPost(
        post.id,
        status,
        this.courseOfferingId
      )
      .subscribe({

        next: () => {

          post.isPinned = status;

          this.toast.success(
            status
              ? 'Post pinned.'
              : 'Post unpinned.'
          );

          this.groupPosts(
            this.filtered
          );

        }

      });

  }

  favoritePost(post: IPostDetail): void {

    const status =
      !post.isFavorite;

    this.postService
      .favoritePost(
        post.id,
        status,
        this.courseOfferingId
      )
      .subscribe({

        next: () => {

          post.isFavorite = status;

          this.toast.success(
            status
              ? 'Added to favorites.'
              : 'Removed from favorites.'
          );

          this.groupPosts(
            this.filtered
          );

        }

      });

  }

  markAsReadUnread(post: IPostDetail): void {

    const status =
      !post.isReaded;

    this.postService
      .markAsReadUnread(
        post.id,
        status,
        this.courseOfferingId
      )
      .subscribe({

        next: () => {

          post.isReaded = status;

          this.toast.success(
            status
              ? 'Marked as read.'
              : 'Marked as unread.'
          );

          this.groupPosts(
            this.filtered
          );

        }

      });

  }

  archivePost(post: IPostDetail): void {

    this.postService
      .archivePost(post.id)
      .subscribe({

        next: () => {

          this.toast.success(
            'Post archived.'
          );

          this.getAllPosts(
            this.courseOfferingId
          );

        }

      });

  }

  deleteFromEveryone(post: IPostDetail): void {

    if (!confirm(
      'Are you sure you want to delete this post?'
    )) {
      return;
    }

    this.postService
      .deletePost(post.id)
      .subscribe({

        next: () => {

          this.toast.success(
            'Post deleted.'
          );

          this.getAllPosts(
            this.courseOfferingId
          );

        }

      });

  }

  toggleGroup(group: any): void {

    group.expanded =
      !group.expanded;

  }

  get totalPages(): number {

    return Math.ceil(
      this.totalItems / this.pageSize
    );

  }

  changePageSize(size: number): void {

    this.pageSize = size;
    this.currentPage = 1;

    this.updatePage();

  }

  goToPage(page: number): void {

    if (
      page < 1 ||
      page > this.totalPages
    ) {
      return;
    }

    this.currentPage = page;

    this.updatePage();

  }

}