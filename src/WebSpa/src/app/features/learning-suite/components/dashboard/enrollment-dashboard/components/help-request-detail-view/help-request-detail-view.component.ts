import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { LabRequestService } from '../../../services/lab-request.service';
import { IHelpRequest, IHelpRequestDetail } from '../../../interfaces/iHelpRequest';

@Component({
  selector: 'app-help-request-detail-view',
  imports: [CommonModule, FormsModule],
  templateUrl: './help-request-detail-view.component.html',
  styleUrl: './help-request-detail-view.component.scss'
})

export class HelpRequestDetailViewComponent implements OnInit {
  EMPTY_ID = '00000000-0000-0000-0000-000000000000';
  initilize: IHelpRequestDetail = {
    id: this.EMPTY_ID,
    requestNumber: "",
    zoomLink: "",
    issueTitle: "",
    issueDescription: "",
    trySofar: "",
    studentId: "",
    studentName: "",
    courseOfferingId: "",
    courseOfferingName: "",
    requestedDate: "",
    assignedTeacherId: "",
    assignedTeacherName: "",
    statusId: "",
    statusName: "",
    dailyNumber: 0
  };

  request: IHelpRequestDetail = this.initilize;

  loading = true;

  statusList = [
    { id: 'pending-id', name: 'Pending' },
    { id: 'approved-id', name: 'Approved' },
    { id: 'rejected-id', name: 'Rejected' }
  ];

  constructor(
    private route: ActivatedRoute,
    private helpRequestService: LabRequestService
  ) { }

  ngOnInit(): void {
    this.loadDetail()
  }

  loadDetail() {
    this.route.queryParamMap.subscribe(params => {
      const id = params.get('id');

      if (id) {
        this.helpRequestService.getLabRequestById(id).subscribe({
          next: (res) => {
            this.request = res.detailsDto ?? this.initilize;
            this.loading = false;
          },
          error: () => {
            this.loading = false;
          }
        });
      }
    });
  }

  getStatusName(statusId: string) {
    const status = this.statusList.find(s => s.id === statusId);
    return status ? status.name : '-';
  }
}
