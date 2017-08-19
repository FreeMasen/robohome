import {SwitchState} from '../models';
export class Switch {
    constructor(public id: number = 0,
                public state: SwitchState = SwitchState.Off,
                public name: string = 'Not in Use',
                public flips: any[] = [],
                public onPin: number = 0,
                public offPin: number = 0) {}
}