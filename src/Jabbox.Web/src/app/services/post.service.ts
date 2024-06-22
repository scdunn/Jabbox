import { Injectable } from '@angular/core';

//http data retrieval imports
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';

import { Post } from '@models/post.model';
import { environment } from '@environment/environment';

// Contacts Service provides data services to get contact information from remote server.
@Injectable()
export class PostService {

  constructor(private http: HttpClient) { }

  getPosts(username:string): Observable<Post[]> {
    return this.http.get<Post[]>(`${environment.JABBOX_API_URL}Post?userName=${username}`, );
  }

  addPost(body: Post): Observable<Post> {
    let result = this.http.put<Post>(`${environment.JABBOX_API_URL}Post`, body);
    return result;
  }



}
