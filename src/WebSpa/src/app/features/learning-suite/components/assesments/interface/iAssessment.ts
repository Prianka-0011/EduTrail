export interface IAssessment {
  id: string;
  title: string;
  description?: string | null;
  courseId?: string | null;
  courseName?: string | null;
  openDate?: string | null;
  dueDate?: string | null;
  availableCredit?: number | null;
  maxScore?: number | null;
}
