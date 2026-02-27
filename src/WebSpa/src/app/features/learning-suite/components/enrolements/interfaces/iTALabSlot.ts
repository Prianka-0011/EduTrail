export interface ITALabSlot {
  id?: string;
  talabDayId: string;      // FK to TALabDay
  startTime: string;       // "HH:mm"
  endTime: string;         // "HH:mm"
  mode: LabMode;
  remoteLink?: string | null;
  isActive?: boolean;
}
export enum LabMode {
  InPerson = 1,
  Remote = 2,
  Hybrid = 3
}
