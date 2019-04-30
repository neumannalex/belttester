import { Component, OnInit } from '@angular/core';
import { NgbDropdown } from '@ng-bootstrap/ng-bootstrap';
import { ProfileMenuComponent } from '../profile/profile-menu.component';
import { AuthenticationService } from '../_services';
import { fadeInContent } from '@angular/material';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isLoggedIn: boolean = false;
  isMember: boolean = false;
  isManager: boolean = false;
  isAdmin: boolean = false;
  isExpanded = false;

  constructor(private authenticationService: AuthenticationService) {}

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

      this.isAdmin = this.authenticationService.hasRole('Admin');
      this.isManager = this.authenticationService.hasRole('Manager');
      this.isMember = this.authenticationService.hasRole('Member');
    }
    else {
      this.isLoggedIn = false;

      this.isAdmin = false;
      this.isManager = false;
      this.isMember = false;
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
