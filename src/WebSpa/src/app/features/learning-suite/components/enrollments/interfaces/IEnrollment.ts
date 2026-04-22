import { IDropdownItem, IDropdownItemInt } from "../../../../../shared/interface/iDropdownItem";
import { IEnrollmentDetail } from "./IEnrollmentDetail";

export interface IEnrollment {
    detailsDto: IEnrollmentDetail;
    detailsDtoList?: IEnrollmentDetail[];
    users?: IDropdownItem[];
    dropdownMonths?: IDropdownItemInt[]
    
}