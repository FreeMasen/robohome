/**
 * Boiler plate
 */
import {NgModule, enableProdMode} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {HttpModule} from '@angular/http';
// enableProdMode();

import './rxjs';
import {Router} from './router';
/**
 * Custom Components
 */
import {AppComponent} from './component';
import {Dashboard,
        Remotes,
        RemoteEditor,
        SwitchEditor,
        FlipEditor,
        SwitchPlate} from './components';
/**
 * Custom Services
 */
import {Data} from './services';

@NgModule({
    imports: [
        BrowserModule,
        Router,
        HttpModule,
        BrowserAnimationsModule,
        FormsModule,
    ],
    declarations: [
        AppComponent,
        Dashboard,
        Remotes,
        RemoteEditor,
        SwitchEditor,
        FlipEditor,
        SwitchPlate
    ],
    providers: [
        Data
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }