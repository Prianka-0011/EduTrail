import { ITALabDay } from "./iTALabDay";

export interface IEnrolementDetail {
  id: string;
  courseOfferingId: string;
  studentId: string;
  studentName?: string | null;
  enrolledDate : string;
  isTa: boolean;
  taLabDays?: ITALabDay[];
}
