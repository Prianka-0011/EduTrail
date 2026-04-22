import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";
import { ICourseOfferingDetail } from "./ICourseOfferignDetail";

export interface ICourseOffering {
    detailDto?: ICourseOfferingDetail;
    detailDtoList?: ICourseOfferingDetail[];
    courses?: IDropdownItem[];
    terms?: IDropdownItem[];
    instructors?: IDropdownItem[];
}
