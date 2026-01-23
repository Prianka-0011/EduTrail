export interface IAssessment {
  id: string;
  title: string;
  description?: string;
  courseId: string;
  openDate: string;
  dueDate: string;
  maxPoints: number;
  availableCredit: number;
}