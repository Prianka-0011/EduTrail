import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";

export interface IHelpRequest {
  detailsDto?: IHelpRequestDetail,  
  detailsListDto?: IHelpRequestDetail[];
  statusList?: IDropdownItem[];
}

export interface IHelpRequestDetail {
  id: string;
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
  dailyNumber?: number;
}
