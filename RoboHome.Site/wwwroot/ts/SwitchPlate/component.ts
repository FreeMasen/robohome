import {Component, Input, Output, EventEmitter} from '@angular/core';

import {Switch, SwitchState} from '../models';

@Component({
    selector: 'switch-plate',
    templateUrl: '../../templates/switchPlate.html',
    styleUrls: ['../../css/switchPlate.css']
})
export class SwitchPlate {
    @Input()
    sw: Switch;

    @Output()
    flipper = new EventEmitter<number>();

    flip() {
        if (this.sw.state == SwitchState.On) {
            this.sw.state = SwitchState.Off;
        } else {
            this.sw.state = SwitchState.On;
        }
        this.flipper.emit(this.sw.id);
    }

    get isOn(): boolean {
        return this.sw.state === SwitchState.On;
    }
}