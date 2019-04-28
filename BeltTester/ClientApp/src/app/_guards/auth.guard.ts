import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private auth: AuthenticationService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));

    //console.log("Current User:")
    //console.log(currentUser);

    if (currentUser) {
      let expiration: Date = new Date(currentUser.expiration);

      let now: Date = new Date;
      let utc_now: any = Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds());
      let currentDate: Date = new Date(utc_now);

      if (expiration > currentDate) { // logged in and expiration not reached so return true
        console.log("AuthGuard: User is logged in and expiration date is not reached.");
        return true;
      }
      else { // logged in but expiration reached --> log out and re-route to login page
        console.log("AuthGuard: User is logged in and expiration date is reached. User will be logged out and forwarded to login page.");
        this.auth.logout();
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
      }
    }

    // not logged in so redirect to login page with the return url
    console.log("AuthGuard: User is not logged in. User will be logged out and forwarded to login page.");
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
