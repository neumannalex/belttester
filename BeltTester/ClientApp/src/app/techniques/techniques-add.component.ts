import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Observable, concat } from 'rxjs';
import { Router } from '@angular/router'
import { MatSnackBar } from '@angular/material';
import { Technique } from '../_models/technique';
import { DataService } from '../_services/data.service';
import { HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'techniques-add',
  templateUrl: './techniques-add.component.html',
  styleUrls: ['./techniques-add.component.css']
})
export class TechniquesAddComponent implements OnInit {
  formGroup: FormGroup;
  name = new FormControl('', [Validators.required]);
  level = new FormControl('', [Validators.required]);
  weapon = new FormControl('', [Validators.required]);
  purpose = new FormControl('', [Validators.required]);
  post: any = '';

  constructor(private formBuilder: FormBuilder, private router: Router, private service: DataService, private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.createForm();
  }

  createForm() {
    this.formGroup = this.formBuilder.group({
      name: this.name,
      level: this.level,
      weapon: this.weapon,
      purpose: this.purpose
    });
  }

  save(item) {
    let technique: Technique = <Technique>item;
    technique.id = 0;

    this.service.createTechnique(technique)
      .subscribe(
        (data: Technique) => {
          this.router.navigate(['/techniques']);
        },
        (err: any) => {
          console.log(err);
          let snackBarRef = this.snackBar.open('Technik konnte nicht gespeichert werden (' + err + ').', 'X', { duration: 5000, verticalPosition: 'top' });
        }
      );
  }

  cancel(form) {
    this.router.navigate(['/techniques']);
  }
}
