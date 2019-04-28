import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router'
import { MatPaginator, MatSort, MatTableDataSource, MatDialog, MatSnackBar } from '@angular/material';
import { YesNoDialog, YesNoDialogParameters } from '../common/yes-no-dialog.component';
import { DataService } from '../_services/data.service';
import { Move } from '../_models/move';

@Component({
  selector: 'moves-list',
  templateUrl: 'moves-list.component.html',
  styleUrls: ['moves-list.component.css']
})
export class MovesListComponent implements OnInit {
  displayedColumns = ['id', 'name', 'symbol', 'actions'];
  dataSource: MatTableDataSource<Move>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private dataService: DataService, private snackBar: MatSnackBar, private router: Router, public dialog: MatDialog) { }

  ngOnInit() {
    this.loadData();
  }

  private loadData() {
    this.dataService.getAllMoves().subscribe((data: Move[]) => {
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

  deleteItem(item: Move) {
    const dialogRef = this.dialog.open(YesNoDialog, {
      maxWidth: '350px',
      data: { title:"Schritt löschen", message: "Schritt wirklich löschen? Der Vorgang kann nicht rückgängig gemacht werden!"}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.dataService.deleteMove(item.id)
          .subscribe(
            (data: any) => {
              this.loadData();
            },
            (err: any) => {
              let snackBarRef = this.snackBar.open('Schritt konnte nicht gelöscht werden.', 'Bitte versuchen sie es später erneut.', { duration: 5000, verticalPosition: 'top' });
              console.log(err.error);
            }
          );
      }
    });
  }
}
