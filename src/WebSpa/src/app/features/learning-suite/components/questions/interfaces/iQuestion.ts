import { IVariationRule } from "./iVariationRule";
import { IQuestionLine } from "./iVariationRule copy";

export interface IQuestion {
  id: string;
  title: string;
  template: string;
  language: string;
  variationRules?: IVariationRule[];
  lines?: IQuestionLine[];
}