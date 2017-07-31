import {Switch} from '../models';
export class Remote {
    constructor(public id: number = -1,
                public location: string = 'none',
                public switches: Switch[] = []) {}
}