/**
 * Boiler plate
 */
import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {HttpModule} from '@angular/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

import './rxjs';
import {Router} from './router';
/**
 * Custom Components
 */
import {AppComponent} from './component';
import {Dashboard,
        Remotes} from './components';
/**
 * Custom Services
 */
import {Data} from './services';

@NgModule({
    imports: [
        BrowserModule,
        Router,
        HttpModule,
        BrowserAnimationsModule
    ],
    declarations: [
        AppComponent,
        Dashboard,
        Remotes
    ],
    providers: [
        Data
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }