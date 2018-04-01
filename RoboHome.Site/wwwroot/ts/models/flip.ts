import {SwitchState, Time} from '../models';
export class Flip {
    constructor(public id: number = -1,
                public direction: SwitchState = SwitchState.Off,
                public time: Time = new Time()) {}
    static fromJson(json: any): Flip {
        if (!json) return new Flip();
        return new Flip(
            json.id,
            json.direction,
            Time.fromJson(json.time),
        )
    }
}