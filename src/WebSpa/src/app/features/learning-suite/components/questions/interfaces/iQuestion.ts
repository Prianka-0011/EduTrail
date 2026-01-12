import { IVariationRule } from "./iVariationRule";
import { IQuestionLine } from "./iQuestionLine";
import { IQuestionDetail } from "./iQuestionDetail";
import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";

export interface IQuestion {
 details: IQuestionDetail;
 types: IDropdownItem[];
 assesments: IDropdownItem [];
}