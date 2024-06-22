import { HttpErrorResponse } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Post } from '@models/post.model';
import { PostService } from '@services/post.service';
import { AuthService } from '@services/auth.service';
import { BaseComponent } from '../shared/base.component';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent extends BaseComponent implements OnInit {
  
  postForm: FormGroup;
  errorMessage: string = '';
  showError: boolean;

  @Output() addPostEvent = new EventEmitter<Post>();

  constructor(private postService: PostService, private router: Router, activatedRoute: ActivatedRoute, authService:AuthService) {
    super(activatedRoute, authService);
  }

  displayStyle = "none";

  openPopup() {
    this.displayStyle = "block";
  }
  closePopup() {
    this.displayStyle = "none";
  } 

  ngOnInit(): void {
    this.postForm = new FormGroup({
      message: new FormControl("", [Validators.required])
    })
 
  }

  validateControl = (controlName: string) => {
    return this.postForm.get(controlName).invalid && this.postForm.get(controlName).touched
  }

  hasError = (controlName: string, errorName: string) => {
    return this.postForm.get(controlName).hasError(errorName)
  }

  addPost = (postFormValue: any) => {
    this.showError = false;
    const post = { ...postFormValue };

    const postModel: Post = {
      message: post.message,
    };

    this.postService.addPost(postModel)
      .subscribe({
        next: (res: Post) => {
          this.addPostEvent.emit(res);
          this.closePopup();
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = err.message;
          this.showError = true;
        }
      })
  }
}
