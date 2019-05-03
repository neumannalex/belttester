import { Component, OnInit, ViewEncapsulation, OnChanges } from '@angular/core';
import { MatDialog, MatSnackBar, MatListOption } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Program } from '../_models/program';
import { DataService } from '../_services/data.service';

@Component({
  selector: 'program-overview',
  templateUrl: 'program-overview.component.html',
  styleUrls: ['program-overview.component.css']
})
export class ProgramOverviewComponent implements OnInit  {
  selectedPrograms: Program[] = [];
  programs: Program[] = [];
  loaded: Boolean = false;

  constructor(private dataService: DataService, private snackBar: MatSnackBar, private router: Router, public dialog: MatDialog, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.loadData();
    });
  }  

  private loadData() {
    this.dataService.getAllPrograms().subscribe((data: Program[]) => {
      data.sort(this.compareByGraduation);
      this.programs = data;
      this.loaded = true;
    },
      (error: any) => {
        this.loaded = false;
        let snackBarRef = this.snackBar.open('Daten konnten nicht geladen werden.', 'Bitte versuchen sie es später erneut.', { duration: 5000, verticalPosition: 'top' });
        snackBarRef.afterDismissed().subscribe(() => {
          this.router.navigate(['/']);
        });
      }
    );
  }

  onNgModelChange(event) {
    //console.log("Event", event);
    //console.log("selectedOptions", this.selectedPrograms);
  }

  showSelectedPrograms() {
    let ids = this.selectedPrograms.join(',');

    //console.log("selectedOptions", this.selectedPrograms);
    this.router.navigate(['/programs/' + ids]);
  }

  private compareByGraduation(a: Program, b: Program) {
    if (a === null || b === null)
      return 0;

    let aValue: number = a.graduationType.toLowerCase() == 'kyu' ? a.graduation + 1000 : a.graduation;
    let bValue: number = b.graduationType.toLowerCase() == 'kyu' ? b.graduation + 1000 : b.graduation;

    if (aValue < bValue) {
      return 1;
    }
    if (aValue > bValue) {
      return -1;
    }
    return 0;
  }
}
