export interface IEnrolementDetail {
  id: string;
  courseOfferingId: string;
  studentId: string;
  studentName?: string | null;
  enrollmentDate: string;
  isTa: boolean;
}
