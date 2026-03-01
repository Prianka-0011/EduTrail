import { IDropdownItem, IDropdownItemInt } from "../../../../../shared/interface/iDropdownItem";
import { IEnrolementDetail } from "./iEntolementDetail";

export interface IEnrolement {
    detailsDto: IEnrolementDetail;
    detailsDtoList?: IEnrolementDetail[];
    users?: IDropdownItem[];
    dropdownMonths?: IDropdownItemInt[]
    
}