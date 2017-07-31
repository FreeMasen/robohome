import {NgModule} from '@angular/core';
import {RouterModule, Route} from '@angular/router';

import {Dashboard, Remotes, Remote} from './components';

const routes: Route[] = [
    {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
    {path: 'dashboard', component: Dashboard},
    {path: 'remotes', component: Remotes},
    {path: 'remote/:id', component: Remote}
]

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class Router {  }