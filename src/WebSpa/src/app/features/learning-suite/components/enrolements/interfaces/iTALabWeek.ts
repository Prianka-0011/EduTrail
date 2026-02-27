import { ITALabDay } from "./iTALabDay";

export interface ITALabWeek {
    id?: string;
    weekNumber: number;
    taLabMonthId?: string;
    days: ITALabDay[];
}