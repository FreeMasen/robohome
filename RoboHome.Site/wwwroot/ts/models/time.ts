import {TimeOfDay} from '../models';

export class Time {
    constructor(
        public hour: number = 0,
        public minute: number = 0,
        public timeOfDay: TimeOfDay = TimeOfDay.Am
    ) {}
    static fromJson(json: any): Time {
        if (!json) return new Time();
        return new Time(
            json.hour,
            json.minute,
            json.timeOfDay
        );
    }
}