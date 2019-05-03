import { Component, OnInit, ViewEncapsulation, OnChanges, Input } from '@angular/core';
import { MatDialog, MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Program } from '../_models/program';
import { DataService } from '../_services/data.service';
import { of, from, Observable, forkJoin } from 'rxjs';
import { map, mergeAll, concatMap, mergeMap } from 'rxjs/operators';

@Component({
  selector: 'program-view',
  templateUrl: 'program-view.component.html',
  styleUrls: ['program-view.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ProgramViewComponent implements OnInit  {
  program: Program;
  loaded: Boolean = false;

  constructor(private dataService: DataService, private snackBar: MatSnackBar, private router: Router, public dialog: MatDialog, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      let ids = (<string>this.route.snapshot.params.id).split(',');
      //console.log("ids:", ids);

      this.loadAndMergeData(ids);
    });
  }  

  private loadData(id: number) {
    this.dataService.getProgram(id).subscribe((data: Program) => {
      this.program = data;
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

  private loadAndMergeData(ids: string[]) {
    let loaded_programs: Program[] = [];

    forkJoin(ids.map(
      id => this.dataService.getProgram(+id)
    )).subscribe((x: Program[]) => {
      //console.log("x", x)
      loaded_programs = x;

      if (loaded_programs.length > 1) {
        // merge programs
        console.log("Found more than 1 program");
        this.program = loaded_programs[loaded_programs.length - 1];
      }
      else {
        console.log("Found exactly 1 program", loaded_programs[0]);
        this.program = loaded_programs[0];
      }

      this.loaded = true;
    });

    //ids.forEach((id: string) => {
    //  this.dataService.getProgram(+id).subscribe((data: Program) => {
    //    console.log("Data:", data);
    //    loaded_programs.push(data);
    //  },
    //    (error: any) => {
    //      this.loaded = false;
    //      let snackBarRef = this.snackBar.open('Daten konnten nicht geladen werden.', 'Bitte versuchen sie es später erneut.', { duration: 5000, verticalPosition: 'top' });
    //      snackBarRef.afterDismissed().subscribe(() => {
    //        this.router.navigate(['/']);
    //      });
    //    }
    //  );
    //});

    //console.log("Loaded programs:", loaded_programs);

    //if (loaded_programs.length > 1) {
    //  // merge programs
    //  console.log("Found more than 1 program");
    //  this.program = loaded_programs[loaded_programs.length - 1];
    //}
    //else {
    //  console.log("Found exactly 1 program", loaded_programs[0]);
    //  this.program = loaded_programs[0];
    //}

    //console.log("Resulting program:", this.program);

    //this.loaded = true;
  }
}
