import { IDropdownItemInt } from "../../../../../shared/interface/iDropdownItem";
import { ITALabMonth } from "../../enrolements/interfaces/iTALabMonth";

export interface IUserEnrolementByCourseOffering {
    dropdownMonths?: IDropdownItemInt[];
    detailsDto: IUserEnrolementByCourseOfferingDetail;
    detailsListDto?: IUserEnrolementByCourseOfferingDetail[];
    year?: Number;
}

export interface IUserEnrolementByCourseOfferingDetail {
    id: string;
    courseOfferingId: string;
    studentId: string;
    studentName?: string | null;
    enrolledDate: string;
    isTa: boolean;
    isActive?: boolean;
    months?: ITALabMonth[];
    totalWorkHoursPerWeek?: number | 10;
}
