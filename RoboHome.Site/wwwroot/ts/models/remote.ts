import {Switch} from '../models';
export class Remote {
    constructor(public id: number = -1,
                public location: string = '',
                public switches: Switch[] = []) {}
    static fromJson(json): Remote {
        if (!json) return new Remote();
        return new Remote(
            json.id,
            json.location,
            json.switches.map(s => Switch.fromJson(s)),
        )
    }
}