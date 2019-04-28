import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Observable, concat } from 'rxjs';
import { Router } from '@angular/router'
import { MatSnackBar } from '@angular/material';
import { Stance } from '../_models/stance';
import { DataService } from '../_services/data.service';
import { HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'stances-add',
  templateUrl: './stances-add.component.html',
  styleUrls: ['./stances-add.component.css']
})
export class StancesAddComponent implements OnInit {
  formGroup: FormGroup;
  name = new FormControl('', [Validators.required]);
  symbol = new FormControl('', [Validators.required]);
  post: any = '';

  constructor(private formBuilder: FormBuilder, private router: Router, private service: DataService, private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.createForm();
  }

  createForm() {
    this.formGroup = this.formBuilder.group({
      name: this.name,
      symbol: this.symbol
    });
  }

  save(item) {
    let stance: Stance = <Stance>item;
    stance.id = 0;

    this.service.createStance(stance)
      .subscribe(
      (data: Stance) => {
          this.router.navigate(['/stances']);
        },
        (err: any) => {
          console.log(err);
          let snackBarRef = this.snackBar.open('Stand konnte nicht gespeichert werden (' + err + ').', 'X', { duration: 5000, verticalPosition: 'top' });
        }
      );
  }

  cancel(form) {
    this.router.navigate(['/stances']);
  }
}
