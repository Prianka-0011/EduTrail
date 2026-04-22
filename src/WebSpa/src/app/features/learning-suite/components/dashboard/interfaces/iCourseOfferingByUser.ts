import { IDropdownItemInt } from "../../../../../shared/interface/iDropdownItem";
import { ICourseOfferingDetail } from "../../course-offerings/interfaces/ICourseOfferignDetail";
import { ITALabMonth } from "../../enrolements/interfaces/ITALabMonth";

export interface ICourseOfferingByUser {
    detailsDtoList?: ICourseOfferingDetail[];
    // detailsDto: IEnrolementDetail;
    // dropdownMonths?: IDropdownItemInt[]
}

export interface ICourseOfferingByUserDetail {
    id: string;
    courseName?: string;
    termName?: string;
    instructorName?: string | null;
}

// export interface IEnrolementDetail {
//     id: string;
//     courseOfferingId: string;
//     studentId: string;
//     studentName?: string | null;
//     enrolledDate: string;
//     isTa: boolean;
//     isActive?: boolean;
//     months?: ITALabMonth[];
//     totalWorkHoursPerWeek?: number | 10;
//     // taLabDays?: ITALabDay[];
// }

