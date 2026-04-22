import { ITALabDay } from "./ITALabDay";

export interface ITALabWeek {
    id?: string;
    weekNumber: number;
    taLabMonthId?: string;
    days: ITALabDay[];
}