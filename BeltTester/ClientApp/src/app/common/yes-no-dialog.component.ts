import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'yes-no-dialog',
  template: `
<h2 mat-dialog-title>{{parameters.title}}</h2>
<div mat-dialog-content>
 {{parameters.message}}
</div>
<div mat-dialog-actions>
  <button mat-button [mat-dialog-close]="true">Ja</button>
  <button mat-button mat-dialog-close cdkFocusInitial>Nein</button>
  <!--<button mat-button (click)="onNoClick()">No</button>-->
  <!--<button mat-button [mat-dialog-close]="true" cdkFocusInitial>Yes</button>-->
</div>
`
})
export class YesNoDialog {

  constructor(public dialogRef: MatDialogRef<YesNoDialog>, @Inject(MAT_DIALOG_DATA) public parameters: YesNoDialogParameters) { }
}

export interface YesNoDialogParameters {
  title: string,
  message: string
}
