import { ITALabDay } from "./ITALabDay";
import { ITALabMonth } from "./ITALabMonth";

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
