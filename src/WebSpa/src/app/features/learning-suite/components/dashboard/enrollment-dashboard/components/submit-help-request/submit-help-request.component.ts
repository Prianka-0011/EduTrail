import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { IHelpRequest, IHelpRequestDetail } from '../../../interfaces/iHelpRequest';
import { UserDashboardService } from '../../../services/user-dashboard.service';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-submit-help-request',
  imports: [CommonModule, FormsModule],
  templateUrl: './submit-help-request.component.html',
  styleUrl: './submit-help-request.component.scss'
})
export class SubmitHelpRequestComponent implements OnInit {
  EMPTY_ID = '00000000-0000-0000-0000-000000000000';
  @Output() saved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  @ViewChild('helpRequestForm') helpRequestForm!: NgForm;

  helpRequest: IHelpRequestDetail = {
    issueTitle: '',
    issueDescription: '',
    trySofar: '',
    zoomLink: '',
    requestedDate: '',
    courseOfferingId: ''
  };

  isIssueTitleFocused = false;
  isIssueDescriptionFocused = false;
  isTryFocused = false;
  isZoomFocused = false;

  constructor(
    private helpRequestService: UserDashboardService,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    const courseOfferingId = this.route.parent?.snapshot.paramMap.get('courseOfferingId');
    this.helpRequest.courseOfferingId = courseOfferingId ?? this.EMPTY_ID
  }

  onSubmit(form: NgForm) {
    if (!form.valid) return;

    const payload: IHelpRequest = {
      detailsDto: {
        ...this.helpRequest,
        requestedDate: new Date().toISOString(), 
      }
    };

    this.helpRequestService.createHelpRequest(payload).subscribe({
      next: () => {
        this.helpRequestForm.resetForm({
          issueTitle: '',
          issueDescription: '',
          trySofar: '',
          zoomLink: ''
        });
        this.saved.emit();
      }
    });
  }

  cancelForm() {
    this.cancel.emit();
  }
}