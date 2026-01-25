export interface IAssessment {
  id: string;
  title: string;
  description?: string | null;
  availableCredit?: number | null;
  maxScore?: number | null;
  courseId?: string | null;
  courseName?: string | null;
  openDate?: string;
  dueDate?: string;
}
