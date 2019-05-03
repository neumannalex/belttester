import { Component, ViewChild, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router'
import { MatPaginator, MatSort, MatTableDataSource, MatDialog, MatSnackBar, fadeInContent } from '@angular/material';
import { YesNoDialog, YesNoDialogParameters } from '../common/yes-no-dialog.component';
import { DataService } from '../_services/data.service';
import { Program, Combination, Motion } from '../_models/program';
import { Stance } from "../_models/stance";
import { Move } from "../_models/move";
import { Technique } from "../_models/technique";
import { retry } from 'rxjs/operators';

@Component({
  selector: 'programs-detail',
  templateUrl: 'programs-detail.component.html',
  styleUrls: ['programs-detail.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ProgramsDetailComponent implements OnInit {
  program: Program;
  loaded: Boolean = false;

  constructor(private dataService: DataService, private snackBar: MatSnackBar, private router: Router, public dialog: MatDialog, private route: ActivatedRoute) { }

  ngOnInit() {
    this.loadData();
  }

  private loadData() {
    this.dataService.getProgram(this.route.snapshot.params.id).subscribe((data: Program) => {
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

  public getFriendlyCombination(combo: Combination): string {
    let lastMotion: Motion = null;
    let friendlies: string[] = [];

    combo.motions.forEach((motion) => {
      if (lastMotion === null) {
        friendlies.push(this.getFriendlyMotion(motion, false));
      }
      else {
        if (lastMotion.stance.symbol == motion.stance.symbol) {
          friendlies.push(this.getFriendlyMotion(motion, true));
        }
        else {
          friendlies.push(this.getFriendlyMotion(motion, false));
        }
      }

      lastMotion = motion;
    });

    return friendlies.join(" ");
  }

  public getFriendlyMotion(motion: Motion, ignoreStance: boolean): string {
    let friendly: string;

    if (!ignoreStance) {
      friendly = "<span class=\"motion\">" + motion.stance.symbol + "</span><span class=\"motion\">" + motion.move.symbol + "</span><span class=\"motion\">" + motion.technique.name + "</span>";

      //friendly = motion.stance.symbol + ' ' + motion.move.symbol + ' ' + motion.technique.name;
    }
    else {
      friendly = "<span class=\"motion\">" + motion.move.symbol + "</span><span class=\"motion\">" + motion.technique.name + "</span>";
      //friendly = motion.move.symbol + ' ' + motion.technique.name;
    }

    if (motion.annotation != '') {
      friendly += "<span class=\"annotation\">(" + motion.annotation + ")</span>";
    }

    return friendly;
  }
}
