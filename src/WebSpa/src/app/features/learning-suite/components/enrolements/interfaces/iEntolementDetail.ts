import { ITALabDay } from "./iTALabDay";
import { ITALabMonth } from "./iTALabMonth";

export interface IEnrolementDetail {
  id: string;
  courseOfferingId: string;
  studentId: string;
  studentName?: string | null;
  enrolledDate: string;
  isTa: boolean;
  taMonths?: ITALabMonth[];
  totalWorkHoursPerWeek?: number | 10;
  // taLabDays?: ITALabDay[];
}
