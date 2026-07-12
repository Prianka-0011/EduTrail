export interface IFolder {
    detailsDto?: IFolderDetail;
    detailsListDto?: IFolderDetail[];
}

export interface IFolderDetail {
    id: string;
    name?: string;
    displayOrder?: Number;
    isActive?: boolean;
    courseOfferingId?: string;
    parentFolderId?: string;

    isEditing?: boolean;
    editName?: string;
    expanded?: boolean;

    showCreateSubFolder?: boolean;
    showSubFolders?: boolean;

    isNumberedSubFolder?: boolean;
    subFolderStart?: number;
    subFolderEnd?: number;
    subFolderName?: string;

    subFolders?: IFolderDetail[];
}