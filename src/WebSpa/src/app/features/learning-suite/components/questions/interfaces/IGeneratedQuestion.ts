import { IQuestionLine } from "./iVariationRule copy";

export interface IGeneratedQuestion {
  questionId: string;
  displayText: string;
  lines: IQuestionLine[];
  parameters: { [key: string]: string };
}