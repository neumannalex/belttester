import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { Move } from '../_models/move';
import { DataService } from '../_services/data.service';

@Component({
  selector: 'moves-add',
  templateUrl: './moves-add.component.html',
  styleUrls: ['./moves-add.component.css']
})
export class MovesAddComponent implements OnInit {
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
    let move: Move = <Move>item;
    move.id = 0;

    this.service.createMove(move)
      .subscribe(
      (data: Move) => {
        this.router.navigate(['/database/moves']);
        },
        (err: any) => {
          console.log(err);
          let snackBarRef = this.snackBar.open('Schritt konnte nicht gespeichert werden (' + err + ').', 'X', { duration: 5000, verticalPosition: 'top' });
        }
      );
  }

  cancel() {
    this.router.navigate(['/database/moves']);
  }
}
