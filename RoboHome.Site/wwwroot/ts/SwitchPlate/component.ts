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
        this.flipper.emit(this.sw.id);
    }

    get isOn(): boolean {
        console.log('SwitchPlate.get isOn', this.sw.state);
        return this.sw.state === SwitchState.On;
    }
}