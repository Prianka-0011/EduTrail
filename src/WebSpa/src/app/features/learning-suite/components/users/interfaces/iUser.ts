import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";
import { IUserDetail } from "./IUserDetail";

export interface IUser {
    detailDto: IUserDetail;
    detailDtoList: IUserDetail[];
    dropdownRoleList: IDropdownItem[];
}