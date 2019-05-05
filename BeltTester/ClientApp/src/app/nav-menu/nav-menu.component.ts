import { Component, OnInit } from '@angular/core';
import { NgbDropdown } from '@ng-bootstrap/ng-bootstrap';
import { ProfileMenuComponent } from '../profile/profile-menu.component';
import { AuthenticationService } from '../_services';
import { fadeInContent } from '@angular/material';
import { Router } from '@angular/router'
import { DataService } from '../_services/data.service';
import { Program, Combination, Motion } from '../_models/program';
import { MatPaginator, MatSort, MatTableDataSource, MatDialog, MatSnackBar, MatSortable } from '@angular/material';
import { Observable } from 'rxjs';

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
  programs: Program[] = [];

  constructor(private authenticationService: AuthenticationService, private dataService: DataService, private snackBar: MatSnackBar, private router: Router) {}

  ngOnInit() {
    this.setAuthenticationState();

    this.authenticationService.isLoggedIn().subscribe(loggedIn => {
      this.setAuthenticationState();
    });

    this.loadPrograms();
  }

  private loadPrograms() {
    this.dataService.getAllPrograms().subscribe((data: Program[]) => {
      data.sort(this.compareByGraduation);

      this.programs = data;
    },
      (error: any) => {
        let snackBarRef = this.snackBar.open('Daten konnten nicht geladen werden.', 'Bitte versuchen sie es später erneut.', { duration: 5000, verticalPosition: 'top' });
        snackBarRef.afterDismissed().subscribe(() => {
          this.router.navigate(['/']);
        });
      }
    );
  }

  private compareByGraduation(a: Program, b: Program) {
    if (a === null || b === null)
      return 0;

    let aValue: number = a.graduationType.toLowerCase() == 'kyu' ? 10 - a.graduation : 100 + a.graduation;
    let bValue: number = b.graduationType.toLowerCase() == 'kyu' ? 10 - b.graduation : 100 + b.graduation;

    if (aValue < bValue) {
      return -1;
    }
    if (aValue > bValue) {
      return 1;
    }
    return 0;
  }

  private setAuthenticationState() {
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
