import { Component, OnInit } from '@angular/core';
import { AuthService } from '@services/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-base',
  template: ` `,
  styles: []
})
export class BaseComponent {

  constructor(public activatedRoute: ActivatedRoute, public authService: AuthService,) { }
  

  public get currentUser(): string {
    return sessionStorage.getItem("username");
  }

  public get currentAccount(): string {
    return this.activatedRoute.snapshot.paramMap.get('id') || '';
  }

  public get isCurrentAccount(): boolean { return this.currentUser == this.currentAccount }

}
