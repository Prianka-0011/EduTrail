import { ITALabDay } from "./iTALabDay";
import { ITALabWeek } from "./iTALabWeek";

export interface ITALabMonth {
  id: string;
  enrollmentId: string;
  month: number; // 1–12
  year: number;
  weeks: ITALabWeek[];
  isCollapsed?: boolean; // For UI state
}
