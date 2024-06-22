import { HttpErrorResponse } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '@services/auth.service';
import { AuthResponse } from '@models/authresponse.model';
import { LoginModel } from '@models/login.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  private returnUrl!: string;

  loginForm: FormGroup;
  errorMessage: string = '';
  showError: boolean;

  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl("", [Validators.required]),
      password: new FormControl("", [Validators.required])
    })
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

  }

  validateControl = (controlName: string) => {
    return this.loginForm.get(controlName).invalid && this.loginForm.get(controlName).touched
  }

  hasError = (controlName: string, errorName: string) => {
    return this.loginForm.get(controlName).hasError(errorName)
  }

  loginAccount = (loginFormValue: any) => {
    this.showError = false;
    const login = { ...loginFormValue };

    const loginModel: LoginModel = {
      username: login.username,
      password: login.password
    };

    this.authService.loginAccount(loginModel)
      .subscribe({
        next: (res: AuthResponse) => {
          this.authService.recordAccount(res.token, res.userName);
          this.authService.sendAuthStateChangeNotification(res.isAuthSuccessful);
          this.router.navigate([loginModel.username]);
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = err.message;
          this.showError = true;
        }
      })
  }
}
