import { IVariationRule } from "./iVariationRule";
import { IQuestionLine } from "./iQuestionLine";

export interface IQuestion {
  id: string;
  title: string;
  questionTypeId: string;
  assessmentId: string
  template: string;
  language: string;
  variationRules?: IVariationRule[];
  lines?: IQuestionLine[];
}