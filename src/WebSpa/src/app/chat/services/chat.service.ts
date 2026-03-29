import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { IMessage } from '../interfaces/iMessage';
import { enviroment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection!: signalR.HubConnection;
  private messagesSubject = new BehaviorSubject<IMessage[]>([]);
  public messages$ = this.messagesSubject.asObservable();
  private connectionReady: Promise<void>;

  constructor() {
    this.connectionReady = this.buildAndStartConnection();
  }

  private async buildAndStartConnection(): Promise<void> {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(enviroment.chatUrl, {
        accessTokenFactory: () => localStorage.getItem('token') || ''
      })
      .withAutomaticReconnect()
      .build();

    this.registerEvents();

    try {
      await this.hubConnection.start();
      console.log('✅ SignalR Connected');
    } catch (err) {
      console.error('❌ SignalR connection error:', err);
      setTimeout(() => this.buildAndStartConnection(), 5000); // retry after 5s
    }
  }

  private registerEvents() {
    this.hubConnection.on('ReceiveMessage', (msg: IMessage) => {
      console.log('📩 ReceiveMessage:', msg);
      const current = this.messagesSubject.value;
      this.messagesSubject.next([...current, msg]);
    });

    this.hubConnection.on('ReceiveHistory', (msgs: IMessage[]) => {
      console.log('📜 ReceiveHistory:', msgs);
      this.messagesSubject.next(msgs);
    });
  }

  async sendMessage(message: IMessage): Promise<void> {
    await this.connectionReady;

    const payload = {
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
      console.error('❌ SendMessage error:', err);
    }
  }

  async loadHistory(receiverId: string): Promise<void> {
    await this.connectionReady;

    console.log('📤 LoadHistory called', receiverId);
    try {
      await this.hubConnection.invoke('LoadHistory', receiverId);
    } catch (err) {
      console.error('❌ LoadHistory error:', err);
    }
  }
}
