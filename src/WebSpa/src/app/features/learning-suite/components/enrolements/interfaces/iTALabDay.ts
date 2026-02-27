import { ITALabSlot } from "./iTALabSlot";

export interface ITALabDay {
  id?: string;   // FK to Enrollment
  labDate?: string;         // "YYYY-MM-DD"
  isActive?: boolean;
  slots?: ITALabSlot[];
  dayName?: string;
  isOverHours?: boolean;
  // Array of slots for this day
}