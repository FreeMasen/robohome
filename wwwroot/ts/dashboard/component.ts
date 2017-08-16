import {Component, OnInit} from '@angular/core';

import {Remote, Switch, SwitchState} from '../models';
import  {Data} from '../services';

@Component({
    selector: 'dashboard',
    templateUrl: '../../templates/dashboard.html',
    styleUrls: ['../../css/dashboard.css']
}
)
export class Dashboard implements OnInit {
    remotes: Remote[] = [];
    constructor(private data: Data){
    }

    ngOnInit(): void {
        this.data
            .getRemotes()
            .then(remotes => {
                this.remotes = remotes;
            });
    }

    flipSwitch(sw: Switch): void {
        console.log('flipSwitch', sw);
        var newState;
        if (sw.state === SwitchState.On) {
            newState = SwitchState.Off;
        } else {
            newState = SwitchState.On;
        }
        this.data.flip(sw.id, newState)
            .then(result => this.flipResponse(result, sw.id))
    }

    flipResponse(result, switchId): void {
        if (result) {
            var switches = this.remotes.reduce((a, b) => {
                return a.concat(b.switches);
            }, [])
            for (var i = 0; i < switches.length; i++) {
                var sw = switches[i];
                if (sw.id === switchId) {
                    if (sw.state == SwitchState.On) {
                        sw.state = SwitchState.Off;
                    } else {
                        sw.state = SwitchState.On;
                    }
                }
            }
        }
    }
}