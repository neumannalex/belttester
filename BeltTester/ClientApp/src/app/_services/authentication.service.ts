import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, retry } from 'rxjs/operators';
import { AuthenticationRequestParameters, AuthenticationResponseParameters } from '../_models/authenticationParameters';
import { User } from '../_models/user';
import { Subject, Observable } from 'rxjs';
import * as JWT from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

  private _loggedIn: boolean = false;
  private observer = new Subject<boolean>();

  public isLoggedIn(): Observable<boolean> {
    return this.observer.asObservable();
  }

  constructor(private http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) { }

  private convertToArrayOfString = (object) => {
    return (typeof object === 'string') ? Array(object) : object;
  }

  login(username: string, password: string) {
    let parameters: AuthenticationRequestParameters =
    {
      username: username,
      password: password
    };

    return this.http.post<AuthenticationResponseParameters>(this._baseUrl + 'api/authentication/token', parameters)
      .pipe(map(auth => {
        // login successful if there's a jwt token in the response
        if (auth && auth.token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes

          let claims = JWT(auth.token);
          //console.log("Claims:");
          //console.log(claims);

          let currentUser: User =
          {
            firstname: claims.given_name,
            lastname: claims.family_name,
            username: claims.sub,
            email: claims.email,
            token: auth.token,
            expiration: auth.expiration,
            roles: this.convertToArrayOfString(claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'])
          }

          //console.log("User:");
          //console.log(currentUser);

          localStorage.setItem('currentUser', JSON.stringify(currentUser));
          this._loggedIn = true;
          this.observer.next(this._loggedIn);

          return currentUser;
        }
      }));
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this._loggedIn = false;
    this.observer.next(this._loggedIn);
  }

  hasRole(roleName: string): boolean {
    let currentUser = <User>JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser) {
      return currentUser.roles.indexOf(roleName) > -1;
    }
    else {
      return false;
    }
  }
}
