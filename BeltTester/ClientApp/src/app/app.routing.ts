import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { TechniquesListComponent } from './techniques/techniques-list.component';
import { TechniquesAddComponent } from './techniques/techniques-add.component'
import { StancesListComponent } from './stances/stances-list.component';
import { StancesAddComponent } from './stances/stances-add.component';

import { MovesListComponent } from './moves/moves-list.component';
import { MovesAddComponent } from './moves/moves-add.component';

import { ProgramsListComponent } from './programs/programs-list.component';
import { ProgramsDetailComponent } from './programs/programs-detail.component';

import { ProgramViewComponent } from './programview/program-view.component';
import { ProgramOverviewComponent } from './programview/program-overview.component';

import { AuthGuard } from './_guards';


const appRoutes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'database/techniques', component: TechniquesListComponent, canActivate: [AuthGuard] },
  { path: 'database/techniques/create', component: TechniquesAddComponent, canActivate: [AuthGuard] },
  { path: 'database/stances', component: StancesListComponent, canActivate: [AuthGuard] },
  { path: 'database/stances/create', component: StancesAddComponent, canActivate: [AuthGuard] },
  { path: 'database/moves', component: MovesListComponent, canActivate: [AuthGuard] },
  { path: 'database/moves/create', component: MovesAddComponent, canActivate: [AuthGuard] },
  { path: 'database/programs', component: ProgramsListComponent, canActivate: [AuthGuard] },
  { path: 'database/programs/:id', component: ProgramsDetailComponent, canActivate: [AuthGuard] },
  { path: 'programs/:id', component: ProgramViewComponent },
  { path: 'programs', component: ProgramOverviewComponent },

  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);
