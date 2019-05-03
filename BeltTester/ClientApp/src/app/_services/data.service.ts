import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { Observable, from, throwError } from 'rxjs';
import { Technique } from '../_models/technique';
import { Stance } from '../_models/stance';
import { Move } from '../_models/move';
import { User } from '../_models';
import { Program } from '../_models/program';
import { catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class DataService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) { }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error('An client side error occurred:', error.error.message);
      return throwError('Es ist ein Fehler aufgetreten. Bitte versuchen Sie es später erneut.');
    } else {
      console.error(`Backend returned code ${error.status}, ` + `body was: ${error.error}`);
      return throwError(error.error);
    }
  };


  getAllTechniques(): Observable<{} | Technique[]> {
    return this.http.get<Technique[]>(this._baseUrl + 'api/techniques/all').pipe(
      catchError(err => this.handleError(err)));
  }

  getTechnique(id: number): Observable<Technique> {
    return this.http.get<Technique>(this._baseUrl + 'api/techniques/' + id);
  }

  createTechnique(item: Technique): Observable<{} | Technique> {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      return this.http.post<Technique>(this._baseUrl + 'api/techniques', item, {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${currentUser.token}`
        })
      })
        .pipe(
          catchError(err => this.handleError(err))
        );
    }
  }

  deleteTechnique(id: number) {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      return this.http.delete<Technique>(this._baseUrl + 'api/techniques/' + id, {
        headers: new HttpHeaders({
          'Authorization': `Bearer ${currentUser.token}`
        })
      }).pipe(
        catchError(err => this.handleError(err))
      );
    }
  }


  getAllStances(): Observable<{} | Stance[]> {
    return this.http.get<Stance[]>(this._baseUrl + 'api/stances/all').pipe(
      catchError(err => this.handleError(err)));
  }

  getStance(id: number): Observable<Stance> {
    return this.http.get<Stance>(this._baseUrl + 'api/stances/' + id);
  }

  createStance(item: Stance): Observable<{} | Stance> {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      return this.http.post<Stance>(this._baseUrl + 'api/stances', item, {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${currentUser.token}`
        })
      })
        .pipe(
        catchError(err => this.handleError(err))
      );
    }
  }

  deleteStance(id: number) {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      return this.http.delete<Stance>(this._baseUrl + 'api/stances/' + id, {
        headers: new HttpHeaders({
          'Authorization': `Bearer ${currentUser.token}`
        })
      }).pipe(
        catchError(err => this.handleError(err))
      );
    }
  }


  getAllMoves(): Observable<{} | Move[]> {
    return this.http.get<Move[]>(this._baseUrl + 'api/moves/all').pipe(
      catchError(err => this.handleError(err)));
  }

  getMove(id: number): Observable<Move> {
    return this.http.get<Move>(this._baseUrl + 'api/moves/' + id);
  }

  createMove(item: Move): Observable<{} | Move> {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      return this.http.post<Move>(this._baseUrl + 'api/moves', item, {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${currentUser.token}`
        })
      })
        .pipe(
          catchError(err => this.handleError(err))
        );
    }
  }

  deleteMove(id: number) {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      return this.http.delete<Move>(this._baseUrl + 'api/moves/' + id, {
        headers: new HttpHeaders({
          'Authorization': `Bearer ${currentUser.token}`
        })
      }).pipe(
        catchError(err => this.handleError(err))
      );
    }
  }



  getAllPrograms(): Observable<{} | Program[]> {
    return this.http.get<Program[]>(this._baseUrl + 'api/belttestprograms/all').pipe(
      catchError(err => this.handleError(err)));
  }

  getProgram(id: number): Observable<Program> {
    return this.http.get<Program>(this._baseUrl + 'api/belttestprograms/' + id);
  }

  deleteProgram(id: number) {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      return this.http.delete<Program>(this._baseUrl + 'api/belttestprograms/' + id, {
        headers: new HttpHeaders({
          'Authorization': `Bearer ${currentUser.token}`
        })
      }).pipe(
        catchError(err => this.handleError(err))
      );
    }
  }
}
