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
    constructor(private data: Data) {

    }

    ngOnInit(): void {
        this.getOrUpdateRemotes()
    }

    flipSwitch(sw: Switch): void {
        console.log('flipSwitch', sw);
        let state = SwitchState.On;
        if (sw.state == SwitchState.On) {
            state = SwitchState.Off;
        }
        this.data.flip(sw.id, state)
            .then(result => this.getOrUpdateRemotes())
    }

    getOrUpdateRemotes() {
        this.data
            .getRemotes()
            .then(remotes => {
                this.remotes = remotes;
            });
    }
}