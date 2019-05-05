import { Component, OnInit, ViewEncapsulation, OnChanges, Input } from '@angular/core';
import { MatDialog, MatSnackBar, fadeInContent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Program, Combination } from '../_models/program';
import { DataService } from '../_services/data.service';
import { of, from, Observable, forkJoin } from 'rxjs';
import { map, mergeAll, concatMap, mergeMap, retry } from 'rxjs/operators';

@Component({
  selector: 'program-view',
  templateUrl: 'program-view.component.html',
  styleUrls: ['program-view.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ProgramViewComponent implements OnInit  {
  //program: Program;
  programs: Program[] = [];
  loaded: Boolean = false;

  constructor(private dataService: DataService, private snackBar: MatSnackBar, private router: Router, public dialog: MatDialog, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      let ids = (<string>this.route.snapshot.params.id).split(',');
      //console.log("ids:", ids);

      this.loadData(ids);
    });
  }  

  private loadData(ids: string[]) {
    let loaded_programs: Program[] = [];

    forkJoin(ids.map(
      id => this.dataService.getProgram(+id)
    )).subscribe((x: Program[]) => {
      //console.log("x", x)
      loaded_programs = x;

      // Nach Sequencenumber sortieren
      loaded_programs.forEach(p => {
        p.kihonCombinations.sort((a: Combination, b: Combination) => {
          if (a.sequenceNumber < b.sequenceNumber) {
            return -1;
          }
          if (a.sequenceNumber > b.sequenceNumber) {
            return 1;
          }
          return 0;
        })
      });

      this.programs = loaded_programs;

      this.loaded = true;
    });
  }

  isAlreadyDisplayed(programId, combinationId) {
    if (this.programs.length <= 0) {
      return false;
    }

    // das erste Programm muss nicht verglichen werden, denn es wurde noch nichts angezeigt
    if (programId == 0) {
      return false;
    }

    //console.log("isAlreadyDisplayed", programId, combinationId);
    //console.log(this.programs[programId]);

    let shown: boolean = false;
    let baseCombination = this.programs[programId].kihonCombinations[combinationId];

    for (let pid = 0; pid < programId; pid++) {
      for (let cid = 0; cid < this.programs[pid].kihonCombinations.length; cid++) {
        let currentCombination = this.programs[pid].kihonCombinations[cid];
        if (this.isCombinationEqual(baseCombination, currentCombination)) {
          shown = true;
        }
      }
    }

    return shown;
  }

  private isCombinationEqual(a: Combination, b: Combination): boolean {
    if (a.motions.length != b.motions.length) {
      return false;
    }

    for (let i = 0; i < a.motions.length; i++) {
      let m1 = a.motions[i];
      let m2 = b.motions[i];

      if (m1.stance.symbol != m2.stance.symbol) {
        return false;
      }

      if (m1.move.symbol != m2.move.symbol) {
        return false;
      }

      if (m1.technique.name != m2.technique.name) {
        return false;
      }

      if (m1.technique.level != m2.technique.level) {
        return false;
      }
    }

    return true;
  }

  private compareProgramsByGraduation(a: Program, b: Program) {
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
}
