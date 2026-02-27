export interface ITALabSlot {
  id?: string;     // FK to TALabDay
  startTime: string;       // "HH:mm"
  endTime: string;         // "HH:mm"
  mode: LabMode;
  remoteLink?: string | null;
  isActive?: boolean;
  taLabDayId?: string; // Optional FK for easier access to the day
}
export enum LabMode {
  InPerson = 1,
  Remote = 2,
  Hybrid = 3
}
