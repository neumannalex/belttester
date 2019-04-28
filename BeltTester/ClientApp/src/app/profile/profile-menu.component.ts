import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../_services';

@Component({
  selector: 'profile-menu',
  templateUrl: './profile-menu.component.html'
})
export class ProfileMenuComponent implements OnInit {
  isLoggedIn: boolean = false;
  username: string;

  constructor(private authenticationService: AuthenticationService, private router: Router) { }

  ngOnInit() {
    this.setAuthenticationState();

    this.authenticationService.isLoggedIn().subscribe(loggedIn => {
      //console.log("Got event from authenticationService.isLoggedIn()");
      this.setAuthenticationState();
    });    
  }

  private setAuthenticationState() {
    //console.log("setAuthenticationState() was called");
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser) {
      this.isLoggedIn = true;
      this.username = currentUser.username;
    }
    else {
      this.isLoggedIn = false;
      this.username = '';
    }
  }

  public login() {
    //console.log("login clicked");
    //alert("Login");
    this.router.navigate(['/login']);
  }

  public logout() {
    //console.log("logout clicked");
    //alert("Logout");
    this.authenticationService.logout();

    this.ngOnInit();

    this.router.navigate(['/']);
  }
}
