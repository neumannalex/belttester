import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router'
import { MatPaginator, MatSort, MatTableDataSource, MatDialog, MatSnackBar } from '@angular/material';
import { YesNoDialog, YesNoDialogParameters } from '../common/yes-no-dialog.component';
import { DataService } from '../_services/data.service';
import { Program, Combination, Motion } from '../_models/program';
import { Stance } from "../_models/stance";
import { Move } from "../_models/move";
import { Technique } from "../_models/technique";

@Component({
  selector: 'programs-list',
  templateUrl: 'programs-list.component.html',
  styleUrls: ['programs-list.component.css']
})
export class ProgramsListComponent implements OnInit {
  displayedColumns = ['id', 'graduation', 'name', 'stylename', 'actions'];
  dataSource: MatTableDataSource<Program>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private dataService: DataService, private snackBar: MatSnackBar, private router: Router, public dialog: MatDialog) { }

  ngOnInit() {
    this.loadData();
  }

  private loadData() {
    this.dataService.getAllPrograms().subscribe((data: Program[]) => {
      this.dataSource = new MatTableDataSource(data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    },
      (error: any) => {
        let snackBarRef = this.snackBar.open('Daten konnten nicht geladen werden.', 'Bitte versuchen sie es später erneut.', { duration: 5000, verticalPosition: 'top' });
        snackBarRef.afterDismissed().subscribe(() => {
          this.router.navigate(['/']);
        });
      }
    );
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  deleteItem(item: Program) {
    const dialogRef = this.dialog.open(YesNoDialog, {
      maxWidth: '350px',
      data: { title: "Programm löschen", message: "Programm wirklich löschen? Der Vorgang kann nicht rückgängig gemacht werden!" }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.dataService.deleteProgram(item.id)
          .subscribe(
            (data: any) => {
              this.loadData();
            },
            (err: any) => {
              let snackBarRef = this.snackBar.open('Programm konnte nicht gelöscht werden.', 'Bitte versuchen sie es später erneut.', { duration: 5000, verticalPosition: 'top' });
              console.log(err.error);
            }
          );
      }
    });
  }
}
