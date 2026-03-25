import { ITALabDay } from "./iTALabDay";
import { ITALabMonth } from "./iTALabMonth";

export interface IEnrolementDetail {
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
