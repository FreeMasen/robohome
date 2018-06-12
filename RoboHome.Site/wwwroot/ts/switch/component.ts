import {Component, Input, Output, EventEmitter} from '@angular/core';
import {Form} from '@angular/forms';

import {Switch, SwitchState, Flip, TimeType} from '../models';

@Component({
    selector: 'switch',
    templateUrl: '../../templates/switch.html',
    styleUrls: ['../../css/switch.css']
})
export class SwitchEditor {
    @Input() sw: Switch;
    @Output()
    switchDeleted = new EventEmitter<any>();

    get flips(): Array<Flip> {
        return this.sw.flips.sort((lhs, rhs) => {
            let ret = 0;
            ret += (lhs.time.timeOfDay - rhs.time.timeOfDay) * 100;
            ret += (lhs.time.hour - rhs.time.hour) * 10;
            ret += lhs.time.minute - rhs.time.minute
            return ret;
        })
    }

    addFlip(): void {
        console.log("SwitchEditor.addFlip")
        this.sw.flips.push(new Flip());
    }

    deleteSelf(): void {
        this.switchDeleted.emit(this.sw);
    }

    deleteFlip(flip: Flip): void {
        console.log('SwitchEditor.deleteFlip', flip);
        var index = this.sw.flips.indexOf(flip);
        this.sw.flips = this.sw.flips.filter(existingFlip => {
            return existingFlip.id != flip.id;
        });
        this.switchDeleted.emit(flip);
    }
}