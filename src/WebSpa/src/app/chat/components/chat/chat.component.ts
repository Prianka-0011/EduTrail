import { Component, Input, OnInit } from '@angular/core';
import { ChatService } from '../../services/chat.service';
import { ICurrentLoginUserDetail } from '../../../features/learning-suite/components/dashboard/interfaces/iCurrentLoginUserDetail';
import { IMessage } from '../../interfaces/iMessage';
import { IEnrolementDetail } from '../../../features/learning-suite/components/enrolements/interfaces/iEntolementDetail';
import { RouterModule, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
   imports: [
    RouterOutlet,
    CommonModule,
    FormsModule,
    RouterModule
  ],
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  @Input() receiver!: IEnrolementDetail;

  user: ICurrentLoginUserDetail = { id: '', fullName: '', email: '', roles: [] };
  message: string = '';
  messages: IMessage[] = [];

  constructor(private chatService: ChatService) { }

  ngOnInit(): void {
    this.chatService.messages$.subscribe(msgs => {
      // filter messages only between current user and selected receiver
      if (this.receiver) {
        this.messages = msgs.filter(m =>
          (m.userId === this.user.id && m.receiverId === this.receiver.id) ||
          (m.userId === this.receiver.id && m.receiverId === this.user.id)
        );
      }
    });
  }

  send() {
    if (!this.message || !this.receiver?.id) return;

    const messageDto: IMessage = {
      userId: this.user.id,
      receiverId: this.receiver.id,
      courseOfferingId: '',
      userName: this.user.fullName,
      message: this.message,
      createdDate: new Date().toISOString()
    };

    this.chatService.sendMessage(messageDto).then(() => this.message = '');
  }
}