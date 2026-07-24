import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  IPostDetail,
  IPostDiscussion
} from '../../../../interfaces/IPost';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { QuillModule } from 'ngx-quill';
import { PostService } from '../../../../services/post.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MatIconModule } from '@angular/material/icon';
import { CustomCategory } from '../../../../../../../../shared/interface/customCategory';

@Component({
  selector: 'app-view-post',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatCheckboxModule,
    MatSlideToggleModule,
    QuillModule,
    MatRadioModule,
    MatIconModule
  ],
  templateUrl: './view-post.component.html',
  styleUrl: './view-post.component.scss'
})
export class ViewPostComponent implements OnInit {


  constructor(
    private postService: PostService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private toast: ToastrService
  ) { }

  getPostTypeIcon(postTypeId?: string): string {

    switch (postTypeId?.toLowerCase()) {

      case CustomCategory.PostTypes.Poll:
        return 'poll';

      case CustomCategory.PostTypes.Question:
        return 'help_outline';

      case CustomCategory.PostTypes.Question:
        return 'description';

      default:
        return 'article';
    }
  }

  EMPTY_ID = '00000000-0000-0000-0000-000000000000';
  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();
  post?: IPostDetail;

  ngOnInit(): void {

    const postId =
      this.activeRoute.snapshot.queryParamMap.get('id')
      ?? this.EMPTY_ID;
    this.loadPost(postId);

  }

  loadPost(postId: string): void {
    if (!postId) {

      console.error(
        'Post id is required'
      );

      return;

    }

    this.postService
      .getViewPostById(postId)
      .subscribe({

        next: (response) => {


          this.post = response.detailsDto!;

          console.log(this.post, "Api post data")
          console.log(
            'Post loaded',
            this.post
          );


        },


        error: (error) => {


          console.error(
            'Failed to load post',
            error
          );


        }

      });


  }



  goBack(): void {

    window.history.back();

  }



  editPost(): void {

    console.log(
      'Edit post clicked',
      this.post?.id
    );

  }



  likePost(): void {

    console.log(
      'Like post'
    );

  }



  likeDiscussion(
    discussion: IPostDiscussion
  ): void {

    discussion.likes =
      (discussion.likes ?? 0) + 1;

  }



  likeReply(
    reply: IPostDiscussion
  ): void {

    reply.likes =
      (reply.likes ?? 0) + 1;

  }



  submitReply(
    discussion: IPostDiscussion
  ): void {

    console.log(
      'Reply submitted',
      discussion
    );

  }



  startDiscussion(): void {

    console.log(
      'New discussion submitted'
    );

  }

}