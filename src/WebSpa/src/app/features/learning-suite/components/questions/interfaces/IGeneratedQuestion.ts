import { IQuestionLine } from "./iQuestionLine";

export interface IGeneratedQuestion {
  questionId: string;
  displayText: string;
  lines: IQuestionLine[];
  parameters: { [key: string]: string };
}