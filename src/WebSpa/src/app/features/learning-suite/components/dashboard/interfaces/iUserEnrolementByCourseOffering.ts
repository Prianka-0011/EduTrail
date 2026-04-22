
import { IDropdownItem, IDropdownItemInt } from "../../../../../shared/interface/iDropdownItem";
import { ITALabMonth } from "../../enrolements/interfaces/ITALabMonth";

export interface IUserEnrolementByCourseOffering {
    dropdownMonths?: IDropdownItemInt[];
    detailsDto: IUserEnrolementByCourseOfferingDetail;
    detailsListDto?: IUserEnrolementByCourseOfferingDetail[];
    year?: Number;
}

export interface IUserEnrolementByCourseOfferingDetail {
    id: string;
    courseOfferingId: string;
    userId: string;
    userName?: string | null;
    enrolledDate: string;
    isTa: boolean;
    isActive?: boolean;
    months?: ITALabMonth[];
    totalWorkHoursPerWeek?: number | 10;
    roles?: IDropdownItem[];
    
}
