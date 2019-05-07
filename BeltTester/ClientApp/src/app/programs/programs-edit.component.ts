import { Component, OnInit, ElementRef, ViewChildren, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators, FormControlName, FormArray } from '@angular/forms';
import { Observable, concat, Subscription } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router'
import { MatSnackBar } from '@angular/material';
import { Program, Combination, Motion } from '../_models/program';
import { Technique } from '../_models/technique';
import { DataService } from '../_services/data.service';
import { HttpErrorResponse } from '@angular/common/http';
import { catchError, retry } from 'rxjs/operators';
import { Stance, Move } from '../_models';
import { CombinationViewComponent } from '../programview/combination-view.component';
import { MccColorPickerService } from 'material-community-components';

@Component({
  selector: 'programs-edit',
  templateUrl: './programs-edit.component.html',
  styleUrls: ['./programs-edit.component.css']
})
export class ProgramsEditComponent implements OnInit, OnDestroy  {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];
  private sub: Subscription;

  programForm: FormGroup;
  program: Program;
  errorMessage: string;
  pageTitle = 'Programm bearbeiten';

  stancesList: Stance[] = [];
  movesList: Move[] = [];
  techniquesList: Technique[] = [];


  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private dataService: DataService, private snackBar: MatSnackBar, private mccColorPickerService: MccColorPickerService) { }

  ngOnInit() {
    console.log("ProgramsEditComponent");

    this.programForm = this.fb.group({
      name: ['', [Validators.required]],
      styleName: ['', [Validators.required]],
      graduation: ['', [Validators.required]],
      graduationType: ['', [Validators.required]],
      graduationColor: ['', [Validators.required]],
      kihonCombinations: this.fb.array([])
    });

    this.dataService.getAllStances().subscribe(data => {
      this.stancesList = <Stance[]>data;
    });

    this.dataService.getAllMoves().subscribe(data => {
      this.movesList = <Move[]>data;
    });

    this.dataService.getAllTechniques().subscribe(data => {
      this.techniquesList = <Technique[]>data;
    });

    this.sub = this.route.paramMap.subscribe(params => {
      const id = +params.get('id');
      if (id > 0) {
        this.getProgram(id);
      }
      else {
        this.createNewProgram();
      }
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  createNewProgram() {
    this.displayProgram({
        id: null,
        graduation: 9,
        graduationType: 'Kyu',
        graduationColor: '#ffffff',
        name: '',
        styleName: 'Shotokan',
        kihonCombinations: []
      });
  }

  getProgram(id: number): void {
    this.dataService.getProgram(id)
      .subscribe(
        (program: Program) => this.displayProgram(program),
        (error: any) => this.errorMessage = <any>error
      );
  }

  displayProgram(program: Program): void {
    if (this.programForm) {
      this.programForm.reset();
    }

    this.program = program;

    if (this.program.id === 0) {
      this.pageTitle = 'Programm hinzufügen';
    }
    else {
      this.pageTitle = `Programm bearbeiten: ${this.program.graduation}. ${this.program.graduationType}`;
    }

    this.programForm.patchValue({
      name: this.program.name,
      styleName: this.program.styleName,
      graduation: this.program.graduation,
      graduationType: this.program.graduationType,
      graduationColor: this.program.graduationColor
    });

    //console.log(this.program.kihonCombinations);

    let kihonCombinationsControl = <FormArray>this.programForm.controls.kihonCombinations;
    this.program.kihonCombinations.forEach((c: Combination) => {
      kihonCombinationsControl.push(this.fb.group({
        id: c.id,
        programId: c.programId,
        sequenceNumber: c.sequenceNumber,
        motions: this.getMotionsAsFormArray(c)
      }))
    });

    console.log("program", this.program);
    console.log("kihonCombinationsControl", kihonCombinationsControl);

    //this.programForm.setControl('kihonCombinations', this.fb.array(this.program.kihonCombinations || []));
  }

  getMotionsAsFormArray(combination: Combination): FormArray {
    let arr = new FormArray([]);

    combination.motions.forEach((m: Motion) => {
      arr.push(this.fb.group({
        id: m.id,
        sequenceNumber: m.sequenceNumber,
        stance: this.fb.group({
          id: m.stance.id,
          name: m.stance.name,
          symbol: m.stance.symbol
        }),
        move: this.fb.group({
          id: m.move.id,
          name: m.move.name,
          symbol: m.move.symbol
        }),
        technique: this.fb.group({
          id: m.technique.id,
          name: m.technique.name,
          level: m.technique.level
        }),
        annotation: m.annotation
      }));
    })

    return arr;
  }

  addCombination() {
    let combinationsListControl = <FormArray>(<FormArray>this.programForm.controls.kihonCombinations);
    let s = combinationsListControl.length + 1;

    combinationsListControl.push(this.fb.group({
      id: 0,
      programId: 0,
      sequenceNumber: s,
      motions: this.fb.array([])
    }));

    this.addMotion(combinationsListControl.length - 1);
  }

  deleteCombination(combinationIdx: number) {
    let combinationsListControl = <FormArray>(<FormArray>this.programForm.controls.kihonCombinations);
    combinationsListControl.removeAt(combinationIdx);
  }

  addMotion(combinationIdx: number) {
    let motionsListControl = <FormArray>(<FormArray>this.programForm.controls.kihonCombinations).at(combinationIdx).get('motions');
    //let s = this.program.kihonCombinations[combinationIdx].motions[this.program.kihonCombinations[combinationIdx].motions.length - 1].sequenceNumber + 1;
    let s = motionsListControl.length + 1;

    motionsListControl.push(this.fb.group({
      id: 0,
      sequenceNumber: s,
      stance: this.fb.group({
        id: 0,
        name: '',
        symbol: ''
      }),
      move: this.fb.group({
        id: 0,
        name: '',
        symbol: ''
      }),
      technique: this.fb.group({
        id: 0,
        name: '',
        level: ''
      }),
      annotation: ''
    }));
  }

  deleteMotion(combinationIdx: number, motionIdx) {
    let motionsListControl = <FormArray>(<FormArray>this.programForm.controls.kihonCombinations).at(combinationIdx).get('motions');
    motionsListControl.removeAt(motionIdx);
  }

  saveProgram() {
    console.log("this.program", this.program);
    console.log("this.programForm.value", this.programForm.value);

    const p = { ...this.program, ...this.programForm.value };
    console.log("p", p);
  }

  test(data: any) {
    //let a = <FormArray>this.programForm.get('kihonCombinations');

    console.log("Data", data);
    //console.log("this.kihonCombinations", a);

    //console.log("a.controls[0]", a.controls[0]);
    //console.log("a.controls[0]['sequenceNumber']", a.controls[0]["value"]["sequenceNumber"]);
  }
}
