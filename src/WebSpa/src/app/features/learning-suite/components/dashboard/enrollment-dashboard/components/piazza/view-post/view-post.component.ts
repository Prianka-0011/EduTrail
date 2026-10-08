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
import { TimeAgoPipe } from '../../../../../../../../shared/pipes/TimeAgoPipe';
import { MatTooltipModule } from '@angular/material/tooltip';

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
    MatIconModule,
    MatTooltipModule,
    MatRadioModule,
    MatIconModule,
    TimeAgoPipe
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
      .getViewPostById(postId, this.courseOfferingId)
      .subscribe({

        next: (response) => {
          console.log("Post Detail", response)
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
    console.log("initial replay", request)
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

  bookmarkPost(): void {

    if (!this.post) {
      return;
    }

    var bookmark = this.post.isBookmarked ?? false

    this.postService
      .bookmarkPost(
        this.post.id,
        !bookmark,
        this.courseOfferingId
      )
      .subscribe({

        next: () => {

          this.post!.isBookmarked =
            !bookmark;

          this.toast.success(
            this.post!.isBookmarked
              ? 'Post bookmarked.'
              : 'Bookmark removed.'
          );

        },

        error: () => {

          this.toast.error(
            'Unable to update bookmark.'
          );

        }

      });

  }

  async likePost(): Promise<void> {

    if (!this.post) {
      return;
    }

    try {
      var like = this.post.isLiked ?? false
      await this.discussionService.likePost(
        this.post.id,
        !like,
        this.courseOfferingId
      );
      this.loadPost(this.post.id)

    }
    catch {

      this.toast.error(
        'Unable to like post.'
      );

    }

  }

  sharePost(): void {

    if (!this.post) {
      return;
    }

    navigator.clipboard.writeText(window.location.href);

    this.postService
      .sharePost(
        this.post.id,
        this.courseOfferingId
      )
      .subscribe({

        next: () => {

          if (this.post) {
            this.post.shareCount =
              (this.post.shareCount ?? 0) + 1;
          }

          this.toast.success(
            'Link copied successfully.'
          );

        },

        error: () => {

          this.toast.error(
            'Unable to share post.'
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

        }

      });

  }

  copyPostLink(): void {

    if (!this.post?.id) {
      return;
    }

    const url =
      `${window.location.origin}${window.location.pathname}?id=${this.post.id}`;

    navigator.clipboard
      .writeText(url)
      .then(() => {

        this.toast.success(
          'Post link copied successfully.'
        );

      })
      .catch(error => {

        console.error(
          'Failed to copy post link:',
          error
        );

        this.toast.error(
          'Unable to copy post link.'
        );

      });
  }
  
  replayToDiscussion(discussion: IPostDiscussion
  ): void {
    discussion.isReplayBoxShown = true;
  }

  async likeDiscussion(
    discussion: IPostDiscussion
  ): Promise<void> {

    console.log(discussion.id, "discussion");
    if (!discussion.id || discussion.isLiking) {
      return;
    }

    discussion.isLiking = true;

    try {

      await this.discussionService.likeDiscussion(
        discussion.id,
        this.courseOfferingId
      );
      if (this.post?.id) {

        this.loadPost(
          this.post.id
        );
      }

    } catch (error) {

      console.error(
        'Discussion like failed',
        error
      );

      this.toast.error(
        'Unable to like discussion.'
      );

    }
    finally {

      discussion.isLiking = false;

    }

  }

  likeReply(
    reply: IPostDiscussion
  ): void {

    this.discussionService
      .likeDiscussion(reply.id!, this.courseOfferingId)
      .catch(error => {

        console.error(
          'Like failed',
          error
        );

      });

  }

  async submitReply(discussion: IPostDiscussion): Promise<void> {

    const text = this.replyText[discussion.id!] ?? '';

    if (!text.trim()) {
      this.toast.warning('Please enter reply.');
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

      await this.discussionService.createDiscussion(request);
      this.replyText[discussion.id!] = '';
      discussion.isReplayBoxShown = false;
      if (this.post?.id) {

        this.loadPost(
          this.post.id
        );
      }

    } catch (error) {
      this.toast.error(
        'Unable to submit reply.'
      );

    }
  }

  getInitials(name?: string | null): string {
    if (!name || !name.trim()) {
      return 'U';
    }

    const names = name.trim().split(/\s+/);

    if (names.length === 1) {
      return names[0][0].toUpperCase();
    }

    return (
      names[0][0] +
      names[names.length - 1][0]
    ).toUpperCase();
  }

  async toggleResolved(
    discussion: IPostDiscussion
  ): Promise<void> {

    if (!discussion.id) {
      return;
    }

    try {

      await this.discussionService.resolveDiscussion(
        discussion.id,
        !discussion.isResolved
      );

    } catch (error) {

      console.error(
        'Resolve discussion failed',
        error
      );

      this.toast.error(
        'Unable to update discussion.'
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