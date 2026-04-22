import { IVariationRule } from "./IVariationRule";
import { IQuestionLine } from "./IQuestionLine";

export interface IQuestionDetail {
  id: string;
  title: string;
  questionTypeId: string;
  assessmentId: string
  template: string;
  language: string;
  variationRules?: IVariationRule[];
  lines?: IQuestionLine[];
}