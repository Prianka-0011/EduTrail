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

      console.log(
        'Post Discussion SignalR connected'
      );

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


  // ==========================================================
  // SIGNALR EVENTS
  // ==========================================================

  private registerEvents(): void {

    this.hubConnection.on(
      'DiscussionCreated',
      (discussion: IPostDiscussion) => {

        console.log(
          'DiscussionCreated',
          discussion
        );

        const current =
          this.discussionsSubject.value;

        // Prevent duplicate
        const alreadyExists =
          this.discussionExists(
            current,
            discussion.id
          );

        if (!alreadyExists) {

          this.discussionsSubject.next([
            ...current,
            discussion
          ]);
        }

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

        const current =
          this.discussionsSubject.value;

        const updated =
          current.map(item =>
            item.id === discussion.id
              ? discussion
              : item
          );

        this.discussionsSubject.next(
          updated
        );

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

        const current =
          this.discussionsSubject.value;

        const filtered =
          current.filter(
            x => x.id !== discussionId
          );

        this.discussionsSubject.next(
          filtered
        );
      }
    );
  }


  // ==========================================================
  // JOIN POST
  // ==========================================================

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


  // ==========================================================
  // LEAVE POST
  // ==========================================================

  async leavePost(
    postId: string
  ): Promise<void> {

    await this.connectionReady;

    try {

      await this.hubConnection.invoke(
        'LeavePost',
        postId
      );

      console.log(
        'Left discussion group:',
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


  // ==========================================================
  // CREATE DISCUSSION
  // ==========================================================

  async createDiscussion(
    request: ICreatePostDiscussionRequest
  ): Promise<void> {

    await this.connectionReady;

    try {

      await this.hubConnection.invoke(
        'CreateDiscussion',
        request
      );

    } catch (error) {

      console.error(
        'CreateDiscussion error:',
        error
      );

      throw error;
    }
  }


  // ==========================================================
  // SET INITIAL DISCUSSIONS
  // ==========================================================

  setDiscussions(
    discussions: IPostDiscussion[]
  ): void {

    this.discussionsSubject.next(
      discussions ?? []
    );
  }


  // ==========================================================
  // GET CURRENT DISCUSSIONS
  // ==========================================================

  getCurrentDiscussions():
    IPostDiscussion[] {

    return this.discussionsSubject.value;
  }


  // ==========================================================
  // DUPLICATE CHECK
  // ==========================================================

  private discussionExists(
    discussions: IPostDiscussion[],
    id?: string
  ): boolean {

    if (!id) {
      return false;
    }

    return discussions.some(
      x => x.id === id
    );
  }


  // ==========================================================
  // CLEAR
  // ==========================================================

  clearDiscussions(): void {

    this.discussionsSubject.next([]);
  }
}