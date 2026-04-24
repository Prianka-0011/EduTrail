import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";

export interface ITerm {
 detailDto: ITermDetails;
 detailDtoList?: ITermDetails[];
 types?: IDropdownItem[]
}

export interface ITermDetails {
  id: string;
  name: string;
  year: number;
  startDate: string;
  endDate: string;
  termTypeId?: string;
}
