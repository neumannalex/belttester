import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS  } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatStepperModule
} from '@angular/material';
import { CdkTableModule } from '@angular/cdk/table';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { OkCancelDialogComponent } from './common/ok-cancel-dialog.component';
import { YesNoDialog } from './common/yes-no-dialog.component';
import { NgbdSortableHeader } from './directives/sortable.directive';
import { LoginComponent } from './login/login.component';
import { ProfileMenuComponent } from './profile/profile-menu.component';

import { TechniquesListComponent } from './techniques/techniques-list.component';
import { TechniquesAddComponent } from './techniques/techniques-add.component'

import { StancesListComponent } from './stances/stances-list.component';
import { StancesAddComponent } from './stances/stances-add.component';

import { MovesListComponent } from './moves/moves-list.component';
import { MovesAddComponent } from './moves/moves-add.component';

import { ProgramsListComponent } from './programs/programs-list.component';
import { ProgramsDetailComponent } from './programs/programs-detail.component';

import { ProgramOverviewComponent } from './programview/program-overview.component';
import { ProgramViewComponent } from './programview/program-view.component';
import { MotionViewComponent } from './programview/motion-view.component';
import { CombinationViewComponent } from './programview/combination-view.component';

import { DataService } from './_services';

import { JwtInterceptor, ErrorInterceptor } from './_interceptors';
import { routing } from './app.routing';


@NgModule({
  declarations: [
    AppComponent,
    NgbdSortableHeader,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    OkCancelDialogComponent,
    LoginComponent,
    TechniquesListComponent,
    TechniquesAddComponent,
    StancesListComponent,
    StancesAddComponent,
    MovesListComponent,
    MovesAddComponent,
    ProgramsListComponent,
    ProgramsDetailComponent,
    ProfileMenuComponent,
    ProgramOverviewComponent,
    ProgramViewComponent,
    MotionViewComponent,
    CombinationViewComponent,
    YesNoDialog
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatStepperModule,
    CdkTableModule,
    routing
  ],
  exports: [
    RouterModule,
    CdkTableModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatStepperModule,
    MatDatepickerModule,
    MatDialogModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    DecimalPipe,
    DataService
  ],
  bootstrap: [
    AppComponent
  ],
  entryComponents: [
    OkCancelDialogComponent,
    YesNoDialog,
    MotionViewComponent,
    CombinationViewComponent
  ]
})
export class AppModule { }
