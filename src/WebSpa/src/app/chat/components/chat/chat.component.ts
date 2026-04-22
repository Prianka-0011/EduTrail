import { Component, Input, OnInit, OnChanges, SimpleChanges, ViewChild, ElementRef, EventEmitter, Output } from '@angular/core';
import { ChatService } from '../../services/chat.service';
import { IMessage } from '../../interfaces/IMessage';
import { IEnrolementDetail } from '../../../features/learning-suite/components/enrolements/interfaces/IEntolementDetail';
import { ICurrentLoginUserDetail } from '../../../features/learning-suite/components/dashboard/interfaces/ICurrentLoginUserDetail';
import { RouterModule, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chat',
  imports: [ RouterOutlet, CommonModule, FormsModule, RouterModule ],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnChanges {

  @Input() receiver!: IEnrolementDetail;
  @Input() sender!: ICurrentLoginUserDetail;
  @Input() courseOfferingId!: string;

  @Output() onClose = new EventEmitter<void>();

  @ViewChild('messagesContainer') messagesContainer!: ElementRef;

  message: string = '';
  messages: IMessage[] = [];

  constructor(private chatService: ChatService) { }

  ngOnInit(): void {
  
    this.chatService.messages$.subscribe(msgs => {
      if (!this.receiver) return;

      this.messages = msgs.filter(m =>
        (m.userId === this.sender.id && m.receiverId === this.receiver.userId) ||
        (m.userId === this.receiver.userId && m.receiverId === this.sender.id)
      );

      setTimeout(() => {
        if (this.messagesContainer) {
          this.messagesContainer.nativeElement.scrollTop =
            this.messagesContainer.nativeElement.scrollHeight;
        }
      });
    });

    this.chatService.loadHistory(this.receiver.userId);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['receiver'] && this.receiver?.userId) {
      this.chatService.loadHistory(this.receiver.userId);
    }
  }

  send() {
    if (!this.message || !this.receiver?.userId) return;

    const msg: IMessage = {
      userId: this.sender.id,
      receiverId: this.receiver.userId,
      courseOfferingId: this.courseOfferingId,
      userName: this.sender.fullName,
      message: this.message,
      createdDate: new Date().toISOString()
    };

    this.chatService.sendMessage(msg).then(() => {
      this.message = '';
    });
  }

  closeChat() {
    this.onClose.emit();
  }
}
