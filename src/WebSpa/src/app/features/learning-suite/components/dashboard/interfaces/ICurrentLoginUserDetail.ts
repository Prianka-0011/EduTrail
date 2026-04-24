export interface ICurrentLoginUserDetail
{
    id: string;
    fullName: string;
    image?: string;
    email: string;
    roles?:IRole[];
}

export interface IRole
{
    id: string;
    name: string;
}
