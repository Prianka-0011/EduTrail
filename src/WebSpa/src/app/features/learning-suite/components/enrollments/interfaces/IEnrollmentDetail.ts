import { ITALabMonth } from "./ITALabMonth";

export interface IEnrollmentDetail {
  id: string;
  courseOfferingId: string;
  userId: string;
  studentName?: string | null;
  enrolledDate: string;
  isTa: boolean;
  isActive?: boolean;
  months?: ITALabMonth[];
  totalWorkHoursPerWeek?: number | 10;
}
