import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { SideDrawerComponent } from '../../../../../../../../shared/components/side-drawer/side-drawer.component';
import { PostService } from '../../../../services/post.service';
import { IPostDetail } from '../../../../interfaces/IPost';
import { NewPostComponent } from '../new-post/new-post.component';


@Component({
  selector: 'app-posts-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SideDrawerComponent,
    NewPostComponent
  ],
  templateUrl: './posts-list.component.html',
  styleUrl: './posts-list.component.scss'
})
export class PostsListComponent implements OnInit {
  EMPTY_ID = '00000000-0000-0000-0000-000000000000';

  constructor(
    private postService: PostService,
    private router: Router,
    private toast: ToastrService,
    private activeRoute: ActivatedRoute
  ) { }


  drawerOpen = false;


  posts: IPostDetail[] = [];
  filtered: IPostDetail[] = [];
  paged: IPostDetail[] = [];


  groupedPosts: {
    title: string;
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
    // const courseOfferingId =
    //   this.activeRoute.parent?.snapshot.paramMap.get('courseOfferingId')
    //   ?? this.EMPTY_ID;
    // this.getAllPosts(courseOfferingId);
    this.loadDummyPosts();

  }



  // getAllPosts() {

  //   this.postService.getAllPosts().subscribe({

  //     next: res => {

  //       this.posts = res.detailsListDto ?? [];

  //       this.applyFilter();

  //     },

  //     error: err => console.error(err)

  //   });

  // }

  getAllPosts(courseOfferingId: string) {
    this.postService.getAllPosts(this.courseOfferingId)
      .subscribe({
        next: res => {
          this.posts = res.detailsListDto ?? [];
          this.applyFilter();
        },
        error: err => console.error(err)
      });
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


    // Update Piazza style grouping
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




  openDetailDrawer(id?: string) {


    this.drawerOpen = true;


    this.router.navigate([], {

      queryParams: { id },

      queryParamsHandling: 'merge'

    });


  }




  closeDrawer() {


    this.drawerOpen = false;


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

  loadDummyPosts() {

    this.posts = [
      {
        id: '1',
        summary: 'Welcome to Computer Science Course',
        details: 'This is an announcement post for all students. Please review the course materials before starting the first assignment.',
        postTypeName: 'Instructor',
        createdDate: '2026-07-11T10:30:00'
      },
      {
        id: '2',
        summary: 'Assignment 1 Discussion',
        details: 'Students can discuss questions related to the first assignment here.',
        postTypeName: 'Discussion',
        createdDate: '2026-07-10T15:20:00'
      },
      {
        id: '3',
        summary: 'Database Design Question',
        details: 'I have a question about normalization and database relationships.',
        postTypeName: 'Student',
        createdDate: '2026-07-10T09:15:00'
      },
      {
        id: '4',
        summary: 'Important Exam Information',
        details: 'The midterm exam will be available next week. Please check the exam guidelines.',
        postTypeName: 'Instructor',
        createdDate: '2026-07-09T12:00:00'
      },
      {
        id: '5',
        summary: 'Project Team Discussion',
        details: 'Team members can use this post to discuss project ideas and progress.',
        postTypeName: 'Group',
        createdDate: '2026-07-08T14:45:00'
      }
    ];

    this.applyFilter();

  }

  openMenuId: string | null = null;


  toggleMenu(id: string) {

    this.openMenuId =
      this.openMenuId === id
        ? null
        : id;

  }


  viewPost(post: IPostDetail): void {
    console.log('View Post:', post);

    // add your navigation logic here
    // this.router.navigate(['/piazza/post', post.id]);
  }

  editPost(post: IPostDetail): void {
    console.log('Edit Post:', post);

    // add your edit logic here
  }

  archivePost(post: IPostDetail): void {
    console.log('Archive Post:', post);
  }

  deleteFromEveryone(post: IPostDetail): void {
    console.log('Delete from everyone:', post);

    // this.deletePost(post.id);
  }

  pinPost(post: IPostDetail): void {
    console.log('Pin Post:', post);
  }

  markAsUnread(post: IPostDetail): void {
    console.log('Mark as unread:', post);
  }
}