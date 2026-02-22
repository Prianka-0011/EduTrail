import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";
import { IEnrolementDetail } from "./iEntolementDetail";

export interface IEnrolement {
    detailDto: IEnrolementDetail;
    detailDtoList: IEnrolementDetail[];
    users: IDropdownItem[];
}