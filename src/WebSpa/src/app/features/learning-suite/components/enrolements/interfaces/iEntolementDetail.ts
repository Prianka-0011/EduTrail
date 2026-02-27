import { ITALabDay } from "./iTALabDay";
import { ITALabMonth } from "./iTALabMonth";

export interface IEnrolementDetail {
  id: string;
  courseOfferingId: string;
  studentId: string;
  studentName?: string | null;
  enrolledDate: string;
  isTa: boolean;
  isActive?: boolean;
  months?: ITALabMonth[];
  totalWorkHoursPerWeek?: number | 10;
  // taLabDays?: ITALabDay[];
}
