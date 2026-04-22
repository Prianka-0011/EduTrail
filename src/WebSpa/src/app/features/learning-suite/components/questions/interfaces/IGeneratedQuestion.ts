import { IQuestionLine } from "./IQuestionLine";

export interface IGeneratedQuestion {
  questionId: string;
  displayText: string;
  lines: IQuestionLine[];
  parameters: { [key: string]: string };
}