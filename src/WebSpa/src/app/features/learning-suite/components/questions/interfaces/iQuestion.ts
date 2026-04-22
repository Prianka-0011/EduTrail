import { IQuestionDetail } from "./IQuestionDetail";
import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";

export interface IQuestion {
 details: IQuestionDetail;
 types: IDropdownItem[];
 assesments: IDropdownItem [];
}