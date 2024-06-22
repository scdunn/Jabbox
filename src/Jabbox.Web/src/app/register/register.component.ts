import { HttpErrorResponse } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '@services/auth.service';
import { AuthResponse } from '@models/authresponse.model';
import { RegisterModel } from '@models/register.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  private returnUrl!: string;

  registerForm: FormGroup;
  errorMessage: string = '';
  showError: boolean;

  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      username: new FormControl("", [Validators.required]),
      password: new FormControl("", [Validators.required])
    })
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

  }

  validateControl = (controlName: string) => {
    return this.registerForm.get(controlName).invalid && this.registerForm.get(controlName).touched
  }

  hasError = (controlName: string, errorName: string) => {
    return this.registerForm.get(controlName).hasError(errorName)
  }

  registerAccount = (registerFormValue: any) => {
    this.showError = false;
    const register = { ...registerFormValue };

    const registerModel: RegisterModel = {
      username: register.username,
      password: register.password
    };

    this.authService.registerAccount(registerModel)
      .subscribe({
        next: (res: AuthResponse) => {
          this.authService.recordAccount(res.token, res.userName);
          this.authService.sendAuthStateChangeNotification(res.isAuthSuccessful);
          this.router.navigate([registerModel.username]);
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = err.message;
          this.showError = true;
        }
      })
  }
}
