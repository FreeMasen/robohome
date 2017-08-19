import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Data} from '../services';
import {Remote, Switch, SwitchState} from '../models';

@Component({
    selector: 'remotes',
    templateUrl: '../../templates/remotes.html',
    styleUrls: ['../../css/remotes.css']
})
export class Remotes implements OnInit {
    remotes: Remote[] = [];

    constructor(private data: Data,
                private router: Router) {}

    ngOnInit(): void {
        var self = this;
        this.data.getRemotes()
                .then(remotes => this.addRemotes(remotes));
    }

    isOn(sw: Switch): boolean {
        return sw.state === SwitchState.On;
    }

    edit(id?: string): void {
        if (!id) id = 'new';
        this.router.navigate(['remote', id]);
    }

    private addRemotes(remotes: Remote[]): void {
        this.remotes = remotes;
    }
}