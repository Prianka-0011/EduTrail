import { ICourseOfferingDetail } from "../../course-offerings/interfaces/iCourseOfferignDetail";

export interface ICourseOfferingByUser {
    detailsDtoList?: ICourseOfferingDetail[];
    detailsDto: 
}
export interface ICourseOfferingByUserDetail {
    id: string;
    courseName?: string;
    termName?: string;
    instructorName?: string | null;
}
