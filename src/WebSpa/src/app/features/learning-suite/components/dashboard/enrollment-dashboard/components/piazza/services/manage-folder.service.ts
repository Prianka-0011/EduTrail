import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ManageFolderService {
  private baseUrl = environment.baseUrl + 'folders/';

  constructor(private http: HttpClient) {}

  createFolders(payload: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}`, payload);
  }

  getFolders(courseOfferingId?: string): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}${courseOfferingId ? `?courseOfferingId=${courseOfferingId}` : ''}`
    );
  }

  getSubFolders(parentId?: string): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}subfolders${parentId ? `?parentId=${parentId}` : ''}`
    );
  }

  updateFolder(id: string, payload: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}${id}`, payload);
  }
  
  deleteFolders(folderIds: string[]): Observable<any> {
    return this.http.delete<any>(this.baseUrl, {
      body: folderIds
    });
  }
}