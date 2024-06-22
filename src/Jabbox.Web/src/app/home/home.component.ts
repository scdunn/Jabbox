import { Component } from '@angular/core';
import { Post } from '@models/post.model';
import { PostService } from '@services/post.service';
import { ActivatedRoute } from '@angular/router'
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

  public posts!: Post[];
  public id!: string;

  constructor(private postService: PostService, private route: ActivatedRoute) { }

  ngOnInit(): void
  {
      this.id = this.route.snapshot.paramMap.get('id') || "";
      this.postService.getPosts(this.id).subscribe((data: Post[]) => { this.posts = data });
  }

  addPost(post: Post) {
    this.posts.unshift(post);
  }

}
