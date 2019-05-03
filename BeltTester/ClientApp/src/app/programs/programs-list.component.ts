import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router'
import { MatPaginator, MatSort, MatTableDataSource, MatDialog, MatSnackBar, MatSortable } from '@angular/material';
import { YesNoDialog, YesNoDialogParameters } from '../common/yes-no-dialog.component';
import { DataService } from '../_services/data.service';
import { Program, Combination, Motion } from '../_models/program';
import { Stance } from "../_models/stance";
import { Move } from "../_models/move";
import { Technique } from "../_models/technique";
import { concat } from 'rxjs';

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
      data.sort(this.compareByGraduation);

      this.dataSource = new MatTableDataSource(data);
      this.dataSource.paginator = this.paginator;

      //this.sort.sort(<MatSortable>({ id: 'graduation', start: 'desc' }));
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

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  viewItem(item: Program) {
    this.router.navigateByUrl("programs/" + item.id);
  }

  showItemDetails(item: Program) {
    this.router.navigateByUrl("database/programs/" + item.id);
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
