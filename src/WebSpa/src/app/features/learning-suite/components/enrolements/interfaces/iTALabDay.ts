import { ITALabSlot } from "./iTALabSlot";

export interface ITALabDay {
  id?: string;
  enrollmentId: string;    // FK to Enrollment
  labDate: string;         // "YYYY-MM-DD"
  isActive?: boolean;
  slots?: ITALabSlot[];    // Array of slots for this day
}