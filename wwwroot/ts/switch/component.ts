import {Component, Input} from '@angular/core';
import {Form} from '@angular/forms';

import {Switch, SwitchState, Flip} from '../models';

@Component({
    selector: 'switch',
    templateUrl: '../../templates/switch.html',
    styleUrls: ['../../css/switch.css']
})
export class SwitchEditor {
    @Input() sw: Switch;

    addFlip(): void {
        this.sw.flips.push(new Flip());
    }
}