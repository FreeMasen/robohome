import {TimeOfDay} from '../models';

export class Time {
    constructor(
        public hour: number = 0,
        public minute: number = 0,
        public timeOfDay: TimeOfDay = TimeOfDay.Am,
        public timeType: TimeType = TimeType.Custom,
        public weekDay: WeekDay = 0
    ) {}
    static fromJson(json: any): Time {
        if (!json) return new Time();
        return new Time(
            json.hour,
            json.minute,
            json.timeOfDay,
            json.timeType,
            json.dayOfWeek,
        );
    }

    toJSON(): any {
        return {
            hour: this.hour,
            minute: this.minute,
            timeOfDay: this.timeOfDay,
            timeType: this.timeType,
            dayOfWeek: this.weekDay,
        }
    }
}

export enum TimeType {
    Custom,
    Dawn,
    Noon,
    Sunset,
    Dusk,
    Midnight
}

export enum WeekDay {
    Sunday = 1,
    Monday = 2,
    Tuesday = 4,
    Wednesday = 8,
    Thursday = 16,
    Friday = 32,
    Saturday = 64
}