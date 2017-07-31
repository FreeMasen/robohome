import {Component} from '@angular/core';
import {Location} from '@angular/common';
import {Router} from '@angular/router';

import {Button} from './models';

@Component({
    selector: 'robo-home',
    templateUrl: '../templates/index.html',
    styleUrls: ['../css/style.css'] 
})
export class AppComponent { 
    buttons: Button[] = [
        new Button('dashboard', 'Dashboard'),
        new Button('remotes', 'Remotes')
    ];

    constructor(private location: Location,
                private router: Router) {
    }

    currenPath(href: string): boolean {
        return this.location.path() === '/' + href;
    }

    goTo(path: string): void {
        this.router.navigate([path]);
    }
}