import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { IMessage } from '../interfaces/IMessage';
import { enviroment } from '../../../environments/environment';
import {  IEnrollmentDetail } from '../../features/learning-suite/components/enrollments/interfaces/IEnrollmentDetail';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private currentUserId: string = '';

  setCurrentUser(userId: string) {
    this.currentUserId = userId;
  }

  private hubConnection!: signalR.HubConnection;

  private messagesSubject = new BehaviorSubject<IMessage[]>([]);
  public messages$ = this.messagesSubject.asObservable();

  private messageSubject = new Subject<IEnrollmentDetail>();
  public message$ = this.messageSubject.asObservable();

  private connectionReady: Promise<void>;

  constructor() {
    this.connectionReady = this.startConnection();
  }

  private async startConnection(): Promise<void> {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(enviroment.chatUrl, {
        accessTokenFactory: () => localStorage.getItem('token') || ''
      })
      .withAutomaticReconnect()
      .build();

    this.registerEvents();

    try {
      await this.hubConnection.start();
      console.log('SignalR connected');
    } catch (err) {
      console.error(err);
      setTimeout(() => this.startConnection(), 5000);
    }
  }

  private registerEvents() {

    // this.hubConnection.on('ReceiveMessage', (msg: IMessage) => {

    //   const current = this.messagesSubject.value;
    //   this.messagesSubject.next([...current, msg]);

    //   this.messageSubject.next({
    //     id: '',
    //     courseOfferingId: msg.courseOfferingId,
    //     userId: msg.userId,
    //     enrolledDate: '',
    //     isTa: false,
    //     studentName: msg.userName
    //   });

    // });

    this.hubConnection.on('ReceiveMessage', (msg: IMessage) => {

      const current = this.messagesSubject.value;
      this.messagesSubject.next([...current, msg]);

      // ✅ Only trigger dashboard if message is from OTHER user
      if (msg.userId !== this.currentUserId) {
        this.messageSubject.next({
          id: '',
          courseOfferingId: msg.courseOfferingId,
          userId: msg.userId,
          enrolledDate: '',
          isTa: false,
          studentName: msg.userName
        });
      }

    });

    this.hubConnection.on('ReceiveHistory', (msgs: IMessage[]) => {
      this.messagesSubject.next(msgs);
    });
  }

  async sendMessage(message: IMessage): Promise<void> {
    await this.connectionReady;

    const payload: IMessage = {
      userId: message.userId,
      receiverId: message.receiverId,
      courseOfferingId: message.courseOfferingId,
      userName: message.userName,
      message: message.message,
      createdDate: new Date().toISOString()
    };

    try {
      await this.hubConnection.invoke('SendMessage', payload);
    } catch (err) {
      console.error('SendMessage error:', err);
    }
  }

  async loadHistory(receiverId: string): Promise<void> {
    await this.connectionReady;

    try {
      await this.hubConnection.invoke('LoadHistory', receiverId);
    } catch (err) {
      console.error('LoadHistory error:', err);
    }
  }
}
