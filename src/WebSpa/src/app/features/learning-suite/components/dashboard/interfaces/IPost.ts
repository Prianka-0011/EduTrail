import { IDropdownItem } from "../../../../../shared/interface/iDropdownItem";

export interface IPost {
    detailsDto?: IPostDetail;
    detailsListDto?: IPostDetail[];
    enrollements?: IDropdownItem[];
    types?: IDropdownItem[];
    folders?: IFolderTreeDto[];
}

export interface IFolderTreeDto {
    id: string;
    name: string;
    parentId?: string | null;
    childFolders?: IFolderTreeDto[];
}

export interface IPostDetail {
    id: string;

    summary?: string;
    details?: string;

    postTypeId?: string;
    postTypeName?: string;

    editorType?: EditorType;
    isAnnouncement?: boolean;
    sendEmailImmediately?: boolean;
    isScheduled?: boolean;
    courseOfferingId?: string;
    scheduledAt?: Date;
    updateDate? :Date;
    isDeleted?: boolean;
    createdDate?: string;
    visibilityText? : string; 

    // Poll
    poll?: IPoll;
    folderIds?: string[];
    isIndividual?: boolean;
    enrollmentIds?: string[];
    discussions?: IPostDiscussion[];
}

export interface IPoll {
    id?: string;
    question?: string;
    options: IPollOption[];
}

export interface IPollOption {
    id?: string;
    optionText: string;
    voteCount?: number;
}

export enum EditorType {
    RichText = 1,
    PlainText = 2,
    Markdown = 3
}

export interface IPostDiscussion {

    id?: string;

    postId?: string;

    parentDiscussionId?: string | null;

    enrollmentId?: string;


    // Author
    authorName?: string;

    authorEmail?: string;


    content?: string;

    editorType?: EditorType;

    isResolved?: boolean;

    isDeleted?: boolean;

    createdDate?: string;

    updatedDate?: string;

    likes?: number;

    replies?: IPostDiscussion[];
}