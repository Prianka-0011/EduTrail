import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";
import { ICourseOfferingDetail } from "./iCourseOfferignDetail";

export interface ICourseOffering {
    detailDto?: ICourseOfferingDetail;
    detailDtoList?: ICourseOfferingDetail[];
    courses?: IDropdownItem[];
    terms?: IDropdownItem[];
    instructors?: IDropdownItem[];
}
