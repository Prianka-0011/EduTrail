import { Component, Input, OnInit, OnChanges, SimpleChanges, ViewChild, ElementRef } from '@angular/core';
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
export class ChatComponent implements OnInit, OnChanges {
  @Input() receiver!: IEnrolementDetail;
  @Input() sender!: ICurrentLoginUserDetail;
  @Input() courseOfferingId!: string;

  @ViewChild('messagesContainer') messagesContainer!: ElementRef;

  user!: ICurrentLoginUserDetail;
  message: string = '';
  messages: IMessage[] = [];

  constructor(private chatService: ChatService) {}

  ngOnInit(): void {
    this.user = this.sender;

    this.chatService.messages$.subscribe(msgs => {
      if (!this.receiver) return;

      this.messages = msgs.filter(m =>
        (m.userId === this.user.id && m.receiverId === this.receiver.userId) ||
        (m.userId === this.receiver.userId && m.receiverId === this.user.id)
      );

      // Auto scroll to bottom
      setTimeout(() => {
        if (this.messagesContainer) {
          this.messagesContainer.nativeElement.scrollTop = this.messagesContainer.nativeElement.scrollHeight;
        }
      }, 0);
    });

    if (this.receiver?.userId) {
      this.chatService.loadHistory(this.receiver.userId);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['receiver'] && this.receiver?.userId) {
      this.chatService.loadHistory(this.receiver.userId);
    }
  }

  send() {
    if (!this.message || !this.receiver?.userId) return;

    const messageDto: IMessage = {
      userId: this.sender.id,
      receiverId: this.receiver.userId,
      courseOfferingId: this.courseOfferingId,
      userName: this.user.fullName,
      message: this.message,
      createdDate: new Date().toISOString()
    };

    this.chatService.sendMessage(messageDto).then(() => this.message = '');
  }
}