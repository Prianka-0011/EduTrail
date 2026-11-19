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
  bestScore?: number | null;
}

export interface SubmissionResult {
  submissionId: string;
  score: number;
  feedback: Record<string, FieldFeedback>;
  bestScore?: number | null;
}

export interface RunCodeResult {
  compiled: boolean;
  compileOutput: string;
  stdout: string;
  stderr: string;
  timedOut: boolean;
  exitCode: number;
}
