import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../../environments/environment';
import { IPollVote, IPollVoteRequest, IPost, IPostDetail } from '../interfaces/IPost';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  baseUrl = environment.baseUrl + 'posts';

  constructor(private http: HttpClient) { }


  createPost(postDetail: IPostDetail): Observable<IPost> {

    const payload = {
      postDetailDto: {
        id: postDetail.id,

        summary: postDetail.summary,

        details: postDetail.details,

        postTypeId: postDetail.postTypeId,

        postTypeName: postDetail.postTypeName,

        editorType: postDetail.editorType,

        isAnnouncement: postDetail.isAnnouncement,

        sendEmailImmediately: postDetail.sendEmailImmediately,

        isScheduled: postDetail.isScheduled,

        scheduledAt: postDetail.scheduledAt,

        isDeleted: postDetail.isDeleted,
        isIndividual: postDetail.isIndividual,
        poll: postDetail.poll,
        folderIds: postDetail.folderIds,
        enrollmentIds: postDetail.enrollmentIds
      }
    };


    return this.http.post<IPost>(
      `${this.baseUrl}`,
      payload
    );

  }

  getAllPosts(courseOfferingId: string): Observable<IPost> {

    const params = new HttpParams()
      .set('courseOfferingId', courseOfferingId);

    return this.http.get<IPost>(
      this.baseUrl,
      { params }
    );

  }

  getPostById(
    id: string,
    courseOfferingId: string
  ): Observable<IPost> {
    return this.http.get<IPost>(
      `${this.baseUrl}/${id}?courseOfferingId=${courseOfferingId}`
    );

  }

  getViewPostById(id: string) {
    console.log("Post is triggering with id", id)
    return this.http.get<IPost>(
      `${this.baseUrl}/view/${id}`
    );

  }

  updatePost(postDetail: IPostDetail): Observable<IPost> {
    const payload = {

      postDetailDto: {

        id: postDetail.id,

        summary: postDetail.summary,

        details: postDetail.details,

        postTypeId: postDetail.postTypeId,

        postTypeName: postDetail.postTypeName,

        editorType: postDetail.editorType,

        isAnnouncement: postDetail.isAnnouncement,

        sendEmailImmediately: postDetail.sendEmailImmediately,

        isScheduled: postDetail.isScheduled,

        scheduledAt: postDetail.scheduledAt,

        isDeleted: postDetail.isDeleted,

        poll: postDetail.poll,
        folderIds: postDetail.folderIds,
        enrollmentIds: postDetail.enrollmentIds,

        isIndividual: postDetail.isIndividual

      }

    };


    return this.http.put<IPost>(
      `${this.baseUrl}/${postDetail.id}`,
      payload
    );

  }

  votePoll(pollVoteDto: IPollVoteRequest): Observable<IPollVote> {
    const payload = {
      pollVoteDto: {
        pollOptionId: pollVoteDto.pollOptionId,
        courseOfferingId: pollVoteDto.courseOfferingId
      }
    };
    console.log("Poll Payload", pollVoteDto)

    return this.http.post<IPollVote>(
      `${this.baseUrl}/vote`,
      payload
    );

  }

  getPollResults(pollId: string, courseOfferingId: string) {
    return this.http.get<any>(
      `${this.baseUrl}/poll/${pollId}/results?courseOfferingId=${courseOfferingId}`
    );
  }

  deletePost(id: string): Observable<boolean> {

    return this.http.delete<boolean>(
      `${this.baseUrl}/${id}`
    );

  }

}