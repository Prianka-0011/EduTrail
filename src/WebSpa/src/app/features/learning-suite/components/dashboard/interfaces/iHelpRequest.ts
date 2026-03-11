export interface IHelpRequest {
  detailsDto?: IHelpRequestDetail,  
  detailsListDto?: IHelpRequestDetail[];
}

export interface IHelpRequestDetail {
  id?: string;
  requestNumber?: string;
  zoomLink?: string;
  issueTitle?: string;
  issueDescription?: string;
  trySofar?: string;
  studentId?: string;
  studentName?: string;
  courseOfferingId?: string;
  courseOfferingName?: string;
  requestedDate?: string;
  assignedTeacherId?: string;
  assignedTeacherName?: string;
  statusId?: string;
  statusName?: string;
}
