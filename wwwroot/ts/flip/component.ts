import {Component, Input} from '@angular/core';
import {Form} from '@angular/forms';

import {Flip, SwitchState} from '../models';

@Component({
    selector: 'flip',
    templateUrl: '../../templates/flip.html',
    styleUrls: ['../../css/flip.css']
})
export class FlipEditor {
    @Input() flip: Flip;
    
    get direction(): string {
        if (this.flip.direction == SwitchState.On) {
            return "1"
        }
        return "0";
    }

    set direction(state: string) {
        if (state == "1") {
            this.flip.direction = SwitchState.On;
        } else {
            this.flip.direction = SwitchState.Off;
        }
    }

    get test():string {
        return JSON.stringify(this.flip);
    }
}