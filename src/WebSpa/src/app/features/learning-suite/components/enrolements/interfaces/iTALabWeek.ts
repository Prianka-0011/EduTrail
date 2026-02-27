import { ITALabDay } from "./iTALabDay";

export interface ITALabWeek {
    id?: string;
    weekNumber: number;
    days: ITALabDay[];
}