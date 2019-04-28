import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router'
import { MatPaginator, MatSort, MatTableDataSource, MatDialog, MatSnackBar } from '@angular/material';
import { YesNoDialog, YesNoDialogParameters } from '../common/yes-no-dialog.component';
import { DataService } from '../_services/data.service';
import { Technique } from '../_models/technique';

@Component({
  selector: 'techniques-list',
  templateUrl: 'techniques-list.component.html',
  styleUrls: ['techniques-list.component.css']
})
export class TechniquesListComponent implements OnInit {
  displayedColumns = ['id', 'name', 'level', 'weapon', 'purpose', 'actions'];
  dataSource: MatTableDataSource<Technique>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private dataService: DataService, private snackBar: MatSnackBar, private router: Router, public dialog: MatDialog) { }

  ngOnInit() {
    this.loadData();
  }

  private loadData() {
    this.dataService.getAllTechniques().subscribe((data: Technique[]) => {
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

  deleteItem(item: Technique) {
    const dialogRef = this.dialog.open(YesNoDialog, {
      maxWidth: '350px',
      data: { title:"Technik löschen", message: "Technik wirklich löschen? Der Vorgang kann nicht rückgängig gemacht werden!"}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.dataService.deleteTechnique(item.id)
          .subscribe(
            (data: any) => {
              this.loadData();
            },
            (err: any) => {
              let snackBarRef = this.snackBar.open('Technik konnte nicht gelöscht werden.', 'Bitte versuchen sie es später erneut.', { duration: 5000, verticalPosition: 'top' });
              console.log(err.error);
            }
          );
      }
    });
  }
}
