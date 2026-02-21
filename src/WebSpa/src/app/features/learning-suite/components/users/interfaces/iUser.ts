import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";
import { IUserDetail } from "./iUserDetail";

export interface IUser {
    detailDto: IUserDetail;
    detailDtoList: IUserDetail[];
    dropdownRoleList: IDropdownItem[];
}