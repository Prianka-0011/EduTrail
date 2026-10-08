import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';

import {
  IPostDiscussion,
  ICreatePostDiscussionRequest
} from '../interfaces/IPost';
import { environment } from '../../../../../../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PostDiscussionService {

  private hubConnection!: signalR.HubConnection;

  private currentPostId: string | null = null;

  private discussionsSubject =
    new BehaviorSubject<IPostDiscussion[]>([]);

  public discussions$ =
    this.discussionsSubject.asObservable();

  private discussionSubject =
    new Subject<IPostDiscussion>();

  public discussion$ =
    this.discussionSubject.asObservable();

  private connectionReady: Promise<void>;

  constructor() {

    this.connectionReady =
      this.startConnection();

  }

  // ==========================================================
  // SIGNALR CONNECTION
  // ==========================================================

  private async startConnection(): Promise<void> {

    this.hubConnection =
      new signalR.HubConnectionBuilder()
        .withUrl(environment.postDiscussionUrl, {
          accessTokenFactory: () =>
            localStorage.getItem('token') || ''
        })
        .withAutomaticReconnect()
        .build();

    this.registerEvents();

    try {

      await this.hubConnection.start();

    } catch (error) {

      console.error(
        'Post Discussion SignalR connection failed',
        error
      );

      setTimeout(() => {

        this.connectionReady =
          this.startConnection();

      }, 5000);

    }

  }

  private registerEvents(): void {
    this.hubConnection.on(
      'DiscussionCreated',
      (discussion: IPostDiscussion) => {

        console.log(
          'DiscussionCreated',
          discussion
        );

        const discussions =
          [...this.discussionsSubject.value];

        if (discussion.parentDiscussionId) {

          const parent =
            this.findDiscussion(
              discussions,
              discussion.parentDiscussionId
            );

          if (parent) {

            parent.replies =
              parent.replies ?? [];

            const exists =
              parent.replies.some(
                x => x.id === discussion.id
              );

            if (!exists) {

              parent.replies.push(
                discussion
              );

            }

          }

        }
        else {

          const exists =
            discussions.some(
              x => x.id === discussion.id
            );

          if (!exists) {

            discussions.push(
              discussion
            );

          }

        }

        this.discussionsSubject.next(
          [...discussions]
        );

        this.discussionSubject.next(
          discussion
        );

      }
    );

    this.hubConnection.on(
      'DiscussionUpdated',
      (discussion: IPostDiscussion) => {

        console.log(
          'DiscussionUpdated',
          discussion
        );

        const discussions =
          [...this.discussionsSubject.value];

        const existing =
          this.findDiscussion(
            discussions,
            discussion.id!
          );

        if (existing) {

          Object.assign(
            existing,
            discussion
          );

          this.discussionsSubject.next(
            [...discussions]
          );

        }

        this.discussionSubject.next(
          discussion
        );

      }
    );

    this.hubConnection.on(
      'DiscussionDeleted',
      (discussionId: string) => {

        console.log(
          'DiscussionDeleted',
          discussionId
        );

        const discussions =
          [...this.discussionsSubject.value];

        this.removeDiscussion(
          discussions,
          discussionId
        );

        this.discussionsSubject.next(
          [...discussions]
        );

      }
    );

    this.hubConnection.on(
      'DiscussionResolved',
      (discussion: IPostDiscussion) => {

        const discussions =
          [...this.discussionsSubject.value];

        const existing =
          this.findDiscussion(
            discussions,
            discussion.id!
          );

        if (existing) {

          existing.isResolved =
            discussion.isResolved;

          this.discussionsSubject.next(
            [...discussions]
          );

        }

      }
    );

  }

  async joinPost(
    postId: string
  ): Promise<void> {

    await this.connectionReady;

    try {

      this.currentPostId = postId;

      await this.hubConnection.invoke(
        'JoinPost',
        postId
      );

      console.log(
        'Joined discussion group:',
        postId
      );

    } catch (error) {

      console.error(
        'JoinPost error:',
        error
      );

    }

  }

  async leavePost(
    postId: string
  ): Promise<void> {

    await this.connectionReady;

    try {

      await this.hubConnection.invoke(
        'LeavePost',
        postId
      );

      if (
        this.currentPostId === postId
      ) {

        this.currentPostId = null;

      }

    } catch (error) {

      console.error(
        'LeavePost error:',
        error
      );

    }

  }

  async createDiscussion(
    request: ICreatePostDiscussionRequest
  ): Promise<void> {

    await this.connectionReady;

    const payload = {

      discussionDto: {

        postId:
          request.postId,

        courseOfferingId:
          request.courseOfferingId,

        parentDiscussionId:
          request.parentDiscussionId,

        enrollmentId: null,

        content:
          request.content,

        editorType:
          request.editorType

      }

    };

    try {

      await this.hubConnection.invoke(
        'CreateDiscussion',
        payload
      );

    } catch (error) {

      console.error(
        'CreateDiscussion error:',
        error
      );

      throw error;

    }

  }

  private findDiscussion(
    discussions: IPostDiscussion[],
    id: string
  ): IPostDiscussion | null {

    for (const discussion of discussions) {

      if (discussion.id === id) {

        return discussion;

      }

      if (
        discussion.replies &&
        discussion.replies.length
      ) {

        const found =
          this.findDiscussion(
            discussion.replies,
            id
          );

        if (found) {

          return found;

        }

      }

    }

    return null;

  }

  private removeDiscussion(
    discussions: IPostDiscussion[],
    id: string
  ): boolean {

    const index =
      discussions.findIndex(
        x => x.id === id
      );

    if (index >= 0) {
      discussions.splice(
        index,
        1
      );
      return true;
    }

    for (const discussion of discussions) {
      if (
        discussion.replies &&
        this.removeDiscussion(
          discussion.replies,
          id
        )
      ) {
        return true;
      }
    }
    return false;
  }

  setDiscussions(
    discussions: IPostDiscussion[]
  ): void {

    this.discussionsSubject.next(
      discussions ?? []
    );
  }

  getCurrentDiscussions():
    IPostDiscussion[] {
    return this.discussionsSubject.value;
  }

  clearDiscussions(): void {
    this.discussionsSubject.next(
      []
    );
  }

  private async ensureConnection(): Promise<void> {

    if (
      this.hubConnection &&
      this.hubConnection.state === signalR.HubConnectionState.Connected
    ) {
      return;
    }


    if (
      this.hubConnection &&
      this.hubConnection.state === signalR.HubConnectionState.Disconnected
    ) {

      try {

        await this.hubConnection.start();

        console.log(
          'SignalR connection restored'
        );

      } catch (error) {

        console.error(
          'SignalR reconnect failed',
          error
        );

        throw error;

      }

    }

  }

  async likeDiscussion(
    discussionId: string,
    courseOfferingId: string
  ): Promise<void> {

    await this.connectionReady;

    try {

      await this.hubConnection.invoke(
        'LikeDiscussion',
        {
          discussionId,
          courseOfferingId
        }
      );

    } catch (error) {
      console.error(
        'LikeDiscussion error:',
        error
      );
      throw error;
    }
  }

  async resolveDiscussion(
    discussionId: string,
    isResolved: boolean
  ): Promise<void> {

    await this.ensureConnection();

    await this.hubConnection.invoke(
      'ResolveDiscussion',
      {
        discussionId,
        isResolved
      }
    );
  }

  async likePost(
    postId: string,
    isLiked: boolean,
    courseOfferingId: string
  ): Promise<void> {

    await this.connectionReady;

    await this.hubConnection.invoke(
      'LikePost',
      {
        postId,
        isLiked,
        courseOfferingId
      });
  }
}