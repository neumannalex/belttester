"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var router_1 = require("@angular/router");
var home_component_1 = require("./home/home.component");
var login_component_1 = require("./login/login.component");
var techniques_list_component_1 = require("./techniques/techniques-list.component");
var techniques_add_component_1 = require("./techniques/techniques-add.component");
var stances_list_component_1 = require("./stances/stances-list.component");
var stances_add_component_1 = require("./stances/stances-add.component");
var moves_list_component_1 = require("./moves/moves-list.component");
var moves_add_component_1 = require("./moves/moves-add.component");
var programs_list_component_1 = require("./programs/programs-list.component");
var _guards_1 = require("./_guards");
var appRoutes = [
    { path: '', component: home_component_1.HomeComponent, pathMatch: 'full' },
    { path: 'login', component: login_component_1.LoginComponent },
    { path: 'techniques', component: techniques_list_component_1.TechniquesListComponent, canActivate: [_guards_1.AuthGuard] },
    { path: 'techniques/create', component: techniques_add_component_1.TechniquesAddComponent, canActivate: [_guards_1.AuthGuard] },
    { path: 'stances', component: stances_list_component_1.StancesListComponent, canActivate: [_guards_1.AuthGuard] },
    { path: 'stances/create', component: stances_add_component_1.StancesAddComponent, canActivate: [_guards_1.AuthGuard] },
    { path: 'moves', component: moves_list_component_1.MovesListComponent, canActivate: [_guards_1.AuthGuard] },
    { path: 'moves/create', component: moves_add_component_1.MovesAddComponent, canActivate: [_guards_1.AuthGuard] },
    { path: 'programs', component: programs_list_component_1.ProgramsListComponent, canActivate: [_guards_1.AuthGuard] },
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routing.js.map