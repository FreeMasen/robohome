import {SwitchState, Flip} from '../models';
export class Switch {
    constructor(public id: number = 0,
                public state: SwitchState = SwitchState.Off,
                public name: string = 'Not in Use',
                public number: number = 0,
                public flips: Flip[] = [],
                public onPin: number = 0,
                public offPin: number = 0) {}
    static fromJson(json: any): Switch {
        if (!json) return new Switch();
        return new Switch(
            json.id,
            json.state,
            json.name,
            json.number,
            json.flips.map(f => Flip.fromJson(f)),
            json.onPin,
            json.offPin
        )
    }
}