import {SwitchState, TimeOfDay} from '../models';
export class Flip {
    constructor(public id: number = 0,
                public direction: SwitchState = SwitchState.Off,
                public hour: number = 0,
                public minute: number = 0,
                public timeOfDay: TimeOfDay = TimeOfDay.Am) {}
}