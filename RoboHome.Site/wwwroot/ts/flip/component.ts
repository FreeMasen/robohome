import {Component, Input, Output, EventEmitter} from '@angular/core';
import {Form} from '@angular/forms';

import {Flip, SwitchState, TimeOfDay} from '../models';

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
        console.log('FlipEditor.get hour', this.flip);
        return this.flip.time.hour.toString();
    }

    set hour(value: string) {
        console.log('FlipEditor.set hour', value);
        var parsed = parseInt(value);
        if (parsed > 12) {
            parsed = parsed - 12;
        }
        this.flip.time.hour = parsed;
    }

    get minute(): string {
        console.log('FlipEditor.get minute', this.flip);
        return '0' + this.flip.time.minute.toString().substr(-2);
    }

    set minute(value: string) {
        console.log('FlipEditor.set minute', value);
        this.flip.time.minute = parseInt(value);
    }

    get tod(): string {
        console.log('FlipEditor.get tod', this.flip);
        return this.flip.time.timeOfDay.toString();
    }

    set tod(value: string) {
        console.log('FlipEditor.get tod', value);
        if (value == '0') {
            this.flip.time.timeOfDay = TimeOfDay.Am;
            return;
        }
        if (value == '1') {
            this.flip.time.timeOfDay = TimeOfDay.Pm;
            return;
        }
        console.warn('Invalid TOD', value);
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