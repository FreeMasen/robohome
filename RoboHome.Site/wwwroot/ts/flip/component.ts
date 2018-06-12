import {Component, Input, Output, EventEmitter} from '@angular/core';
import {Form} from '@angular/forms';
import {Data} from '../services';
import {Flip, SwitchState, TimeOfDay, TimeType} from '../models';

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
    constructor(private data: Data){}
    deleteSelf(): void {
        this.deleteHandler.emit(this.flip);
    }

    get timeType(): string {
        return this.flip.time.timeType.toString();
    }
    async setSpecialTime(time: number) {
        console.log('setSpecialTime', time, 'from', this.flip.time.timeType);
        this.flip.time.timeType = time;
        switch (this.flip.time.timeType) {
            case TimeType.Dawn:
                let dbDawn = await this.data.getDawn();
                this.flip.time.hour = dbDawn.hour;
                this.flip.time.minute = dbDawn.minute;
                this.flip.time.timeOfDay = dbDawn.timeOfDay;
            break;
            case TimeType.Noon:
                this.flip.time.hour = 12;
                this.flip.time.minute = 0;
                this.flip.time.timeOfDay = TimeOfDay.Pm;
            break;
            case TimeType.Sunset:
                let dbSunset = await this.data.getSunset();
                this.flip.time.hour = dbSunset.hour;
                this.flip.time.minute = dbSunset.minute;
                this.flip.time.timeOfDay = dbSunset.timeOfDay;
            break;
            case TimeType.Dusk:
                let dbDusk = await this.data.getDusk();
                this.flip.time.hour = dbDusk.hour;
                this.flip.time.minute = dbDusk.minute;
                this.flip.time.timeOfDay = dbDusk.timeOfDay;
            break;
            case TimeType.Midnight:
                this.flip.time.hour = 12;
                this.flip.time.minute = 0;
                this.flip.time.timeOfDay = TimeOfDay.Am;
            break;
        }
    }
    set timeType(value: string) {
        try {
            this.flip.time.timeType = parseInt(value);
        } catch (e) {
            console.error('Unable to parse value', e);
        }
    }

    get hour(): string {
        return this.flip.time.hour.toString();
    }

    set hour(value: string) {
        var parsed = parseInt(value);
        if (parsed > 12) {
            parsed = parsed - 12;
        }
        this.flip.time.hour = parsed;
    }

    get minute(): string {
        return '0' + this.flip.time.minute.toString().substr(-2);
    }

    set minute(value: string) {
        this.flip.time.minute = parseInt(value);
    }

    get tod(): string {
        return this.flip.time.timeOfDay.toString();
    }

    set tod(value: string) {
        if (value == '0') {
            this.flip.time.timeOfDay = TimeOfDay.Am;
            return;
        }
        if (value == '1') {
            this.flip.time.timeOfDay = TimeOfDay.Pm;
        }
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

    dow(day: number): boolean {
        return (this.flip.time.weekDay & day) > 0
    }

    toggleDow(day: number) {
        this.flip.time.weekDay ^= day;
    }

    setTimeType(val: number) {
        if (this.flip.time.timeType == TimeType.Custom
            || val == TimeType.Custom) {
            this.flip.time.timeType = val;
        }
    }
}