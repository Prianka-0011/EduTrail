import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { IMessage } from '../interfaces/iMessage';
import { enviroment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection: signalR.HubConnection;
  private messagesSubject = new BehaviorSubject<IMessage[]>([]);
  public messages$ = this.messagesSubject.asObservable();

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${enviroment.chatUrl}/hubs/chat`, {
        withCredentials: true // required if using cookies for auth
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connected to SignalR'))
      .catch(err => console.error('SignalR connection error:', err));

    this.hubConnection.on('ReceiveMessage', (msg: IMessage) => {
      const current = this.messagesSubject.value;
      this.messagesSubject.next([...current, msg]);
    });

    this.hubConnection.on('ReceiveHistory', (msgs: IMessage[]) => {
      this.messagesSubject.next(msgs);
    });
  }

  sendMessage(messageDto: IMessage): Promise<void> {
    return this.hubConnection.invoke('SendMessage', messageDto);
  }

  joinCourse(courseOfferingId: string) {
    return this.hubConnection.invoke('JoinCourse', courseOfferingId)
      .catch(err => console.error('Error joining course:', err));
  }

  loadHistory(receiverId: string) {
    return this.hubConnection.invoke('LoadHistory', receiverId)
      .catch(err => console.error('Error loading history:', err));
  }
}
