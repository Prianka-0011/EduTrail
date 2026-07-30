import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  ICreatePostDiscussionRequest,
  IPostDetail,
  IPostDiscussion
} from '../interfaces/IPost';
import { OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { QuillModule } from 'ngx-quill';
import { PostService } from '../services/post.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MatIconModule } from '@angular/material/icon';
import { CustomCategory } from '../../../../../../../../shared/interface/customCategory';
import { PostDiscussionService } from '../services/post-discussion.service';

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
export class ViewPostComponent implements OnInit, OnDestroy {


  constructor(
    private postService: PostService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private toast: ToastrService,
    private discussionService: PostDiscussionService
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

  courseOfferingId: string = '00000000-0000-0000-0000-000000000000';
  selectedPollOptionId: string | null = null;
  hasVoted = false;
  EMPTY_ID = '00000000-0000-0000-0000-000000000000';
  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();
  post?: IPostDetail;
  newDiscussion = '';
  replyText: { [key: string]: string } = {};
  selectedEditorType = 1;

  ngOnInit(): void {

    this.courseOfferingId =
      this.activeRoute.parent?.snapshot.paramMap.get('courseOfferingId')
      ?? this.EMPTY_ID;

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

          this.discussionService.setDiscussions(
            this.post.discussions ?? []
          );

          this.discussionService.joinPost(
            this.post.id
          );

          this.discussionService.discussions$
            .subscribe(x => {

              if (this.post) {
                this.post.discussions = x;
              }

            });

          if (this.post.poll?.id) {
            this.loadPollResults();
          }

        },
        error: (error) => {
          console.error(
            'Failed to load post',
            error
          );
        }
      });
  }

  ngOnDestroy(): void {

    if (this.post?.id) {

      this.discussionService.leavePost(
        this.post.id
      );
    }
  }

  async startDiscussion(): Promise<void> {

    if (!this.post) {
      return;
    }

    if (!this.newDiscussion.trim()) {

      this.toast.warning(
        'Please enter discussion.'
      );

      return;
    }

    const request: ICreatePostDiscussionRequest = {

      postId: this.post.id,

      courseOfferingId: this.courseOfferingId,

      parentDiscussionId: null,

      enrollmentId: null,

      content: this.newDiscussion,

      editorType: this.selectedEditorType

    };

    try {

      await this.discussionService.createDiscussion(
        request
      );

      this.newDiscussion = '';

    }
    catch {

      this.toast.error(
        'Unable to create discussion.'
      );

    }

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

  async submitReply(
    discussion: IPostDiscussion
  ): Promise<void> {

    const text =
      this.replyText[discussion.id!] ?? '';

    if (!text.trim()) {

      this.toast.warning(
        'Please enter reply.'
      );

      return;
    }

    const request: ICreatePostDiscussionRequest = {

      postId: discussion.postId!,

      parentDiscussionId: discussion.id,

      courseOfferingId: this.courseOfferingId,

      enrollmentId: null,

      content: text,

      editorType: this.selectedEditorType

    };

    try {

      await this.discussionService.createDiscussion(
        request
      );

      this.replyText[discussion.id!] = '';

    }
    catch {

      this.toast.error(
        'Unable to submit reply.'
      );

    }
  }

  votePoll(): void {

    console.log("this.selectedPollOptionId", this.selectedPollOptionId)
    if (!this.selectedPollOptionId) {

      this.toast.warning(
        'Please select an option.'
      );

      return;
    }


    const payload = {
      pollOptionId: this.selectedPollOptionId,
      courseOfferingId: this.courseOfferingId
    };


    this.postService
      .votePoll(payload)
      .subscribe({

        next: () => {

          this.toast.success(
            'Vote submitted successfully'
          );

          if (this.post?.id) {

            this.loadPost(
              this.post.id
            );

          }

        },


        error: (error) => {

          console.error(
            'Poll vote failed',
            error
          );
          this.toast.error(
            "You already voted on this poll"
          );

        }

      });

  }

  getTotalVotes(): number {

    return this.post?.poll?.options.reduce(
      (sum, x) => sum + x.voteCount,
      0
    ) ?? 0;
  }

  getVotePercentage(votes: number): number {

    const total = this.getTotalVotes();

    if (total === 0) {
      return 0;
    }

    return Math.round((votes / total) * 100);
  }

  loadPollResults(): void {

    if (!this.post?.poll?.id) {
      return;
    }

    this.postService
      .getPollResults(this.post.poll.id, this.courseOfferingId)
      .subscribe({

        next: (response) => {

          console.log(
            'Poll result data',
            response
          );

          if (this.post?.poll) {

            this.post.poll.options = response.options;

            // Show progress bars if results exist
            this.hasVoted = response.isCurrentUserVoted;
            this.selectedPollOptionId =
              response.options.find(
                (c: any) => c.isSelectedByCurrentUser
              )?.id ?? null;
          }

        },

        error: (error) => {

          console.error(
            'Failed to load poll results',
            error
          );

        }

      });
  }
}