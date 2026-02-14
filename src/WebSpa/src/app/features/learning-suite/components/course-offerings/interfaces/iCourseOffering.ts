import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";
import { ICourseOfferingDetail } from "./iCourseOfferignDetail";

export interface ICourseOffering {
    detail: ICourseOfferingDetail;
    courses: IDropdownItem[];
    terms: IDropdownItem[];
    instructors: IDropdownItem[];
}
