export interface FieldFeedback {
  isCorrect: boolean;
  score: number;
  message: string;
}

export interface QuestionInstance {
  instanceId: string;
  renderedHtml: string;
  params: Record<string, unknown>;
  showCorrectAnswer: boolean;
}

export interface SubmissionResult {
  submissionId: string;
  score: number;
  feedback: Record<string, FieldFeedback>;
}
