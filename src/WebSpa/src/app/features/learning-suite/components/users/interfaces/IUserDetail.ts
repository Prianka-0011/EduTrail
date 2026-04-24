import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";

export interface IUserDetail {
    id: string;
    firstName: string;
    middleName: string;
    lastName: string;
    email: string;
    password: string;
    isActive: boolean;
    selectedRoleList: IDropdownItem[];
}