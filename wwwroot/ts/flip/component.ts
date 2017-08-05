import {Component, Input, Output, EventEmitter} from '@angular/core';
import {Form} from '@angular/forms';

import {Flip, SwitchState} from '../models';

@Component({
    selector: 'flip',
    templateUrl: '../../templates/flip.html',
    styleUrls: ['../../css/flip.css']
})
export class FlipEditor {
    @Input() 
    flip: Flip;

    @Output()
    deleteHandler = new EventEmitter<Flip>();
    
    deleteSelf(): void {
        console.log('FlipEditor.deleteSelf');
        this.deleteHandler.emit(this.flip);
    }

    get hour(): string {
        return this.flip.hour.toString();
    }

    set hour(value: string) {
        var parsed = parseInt(value);
        if (parsed > 12) {
            parsed = parsed - 12;
        }
        this.flip.hour = parsed;
    }

    get minute(): string {
        return '0' + this.flip.minute.toString().substr(-2);
    }

    set minute(value: string) {
        this.flip.minute = parseInt(value);
    }

    get direction(): string {
        if (this.flip.direction == SwitchState.On) {
            return "1"
        }
        return "0";
    }

    set direction(state: string) {
        console.log('set direction', state, this.flip.id);
        if (state == "1") {
            this.flip.direction = SwitchState.On;
        } else {
            this.flip.direction = SwitchState.Off;
        }
    }

}