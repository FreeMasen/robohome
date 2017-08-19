import {NgModule} from '@angular/core';
import {RouterModule, Route} from '@angular/router';

import {Dashboard, Remotes, RemoteEditor} from './components';

const routes: Route[] = [
    {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
    {path: 'dashboard', component: Dashboard},
    {path: 'remotes', component: Remotes},
    {path: 'remote/:id', component: RemoteEditor}
]

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class Router {  }