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
import {Dashboard} from './components';
/**
 * Custom Services
 */

@NgModule({
    imports: [
        BrowserModule,
        Router,
        HttpModule,
        BrowserAnimationsModule
    ],
    declarations: [
        AppComponent,
        Dashboard
    ],
    providers: [

    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }