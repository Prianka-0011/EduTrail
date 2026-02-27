import { ITALabSlot } from "./iTALabSlot";

export interface ITALabDay {
  id?: string;   // FK to Enrollment
  labDate?: string;         // "YYYY-MM-DD"
  isActive?: boolean;
  slots?: ITALabSlot[];
  dayName?: string;
  taLabWeekId?: string;
  isOverHours?: boolean;
}