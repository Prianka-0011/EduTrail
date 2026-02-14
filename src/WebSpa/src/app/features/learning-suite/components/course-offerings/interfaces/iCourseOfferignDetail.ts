export interface ICourseOfferingDetail {
    id: string;
    courseId?: string;
    courseName?: string;
    termId?: string;
    termName?: string;
    instructorId?: string | null;
    instructorName?: string | null;
}
