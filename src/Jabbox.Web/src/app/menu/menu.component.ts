import { AuthService } from '../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  public isUserAuthenticated: boolean;
  constructor(private authService: AuthService, private router : Router) { }
  ngOnInit(): void {
    this.isUserAuthenticated = localStorage.getItem("token") != null;
    this.authService.authChanged
      .subscribe(res => {
        this.isUserAuthenticated = res;
      })
  }

  public logout = () => {
    this.authService.logout();
    this.router.navigate(["/"]);
  }
}
