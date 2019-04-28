import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, retry } from 'rxjs/operators';
import { AuthenticationRequestParameters, AuthenticationResponseParameters } from '../_models/authenticationParameters';
import { User } from '../_models/user';
import { Subject, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

  private _loggedIn: boolean = false;
  private observer = new Subject<boolean>();

  public isLoggedIn(): Observable<boolean> {
    return this.observer.asObservable();
  }

  constructor(private http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) { }

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

          let currentUser: User =
          {
            username: username,
            token: auth.token,
            expiration: auth.expiration
          }

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
}
