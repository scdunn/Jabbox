import { Injectable } from '@angular/core';

//http data retrieval imports
import { HttpClient } from '@angular/common/http';
import { Subject, Observable } from 'rxjs';
import { LoginModel } from '@models/login.model';
import { AuthResponse } from '@models/authresponse.model';
import { RegisterModel } from '@models/register.model';
import { environment } from '@environment/environment';

// Contacts Service provides data services to get contact information from remote server.
@Injectable()
export class AuthService {

  private authChangeSub = new Subject<boolean>();
  public authChanged = this.authChangeSub.asObservable();
  public currentAccount: string;

  constructor(private http: HttpClient) {
  }

  public sendAuthStateChangeNotification = (isAuthenticated: boolean) => {
    this.authChangeSub.next(isAuthenticated);
  }

  public loginAccount = (body: LoginModel) => {
    return this.http.post<AuthResponse>(environment.JABBOX_API_URL + "accounts/login", body);
  }

  public registerAccount = (body: RegisterModel) => {
    return this.http.post<AuthResponse>(environment.JABBOX_API_URL + "accounts/register", body);
  }

  public recordAccount(token: string, username: string) {
    sessionStorage.setItem("token", token);
    sessionStorage.setItem("username", username);
  }

  public logout = () => {
    sessionStorage.removeItem("token");
    sessionStorage.removeItem("username");
    this.sendAuthStateChangeNotification(false);
  }


}
