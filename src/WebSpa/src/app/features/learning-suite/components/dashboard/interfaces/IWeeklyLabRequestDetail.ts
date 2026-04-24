export interface IWeeklyLabRequest{
 weeklyDataList: IWeeklyLabRequestDetail[];
}
export interface IWeeklyLabRequestDetail {
  week: string;
  weekStartDate: string;
  monday: number;
  tuesday: number;
  wednesday: number;
  thursday: number;
  friday: number;
  saturday: number;
  sunday: number;
  total: number;
}