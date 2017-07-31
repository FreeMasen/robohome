import {Injectable} from '@angular/core';
import {Http} from '@angular/http';

import 'rxjs';

import {Remote, Switch, Flip} from '../models';

@Injectable()
export class Data {
    constructor(private http: Http) {
        console.log('new Data()');
    }

    getRemotes(): Promise<Remote[]> {
        console.log('Data.getRemotes()');
        return this.http.get('/api/remotes')
                        .toPromise()
                        .then(res => {
                            var responseBody = res.json();
                            return responseBody.map(Data.mapRemote);
                        })
                        .catch(e => {
                            throw e;
                        });
    }

    private static mapRemote(dbValue: any): Remote {
        var switches: Switch[] = dbValue.switches.map(Data.mapSwitch);
        return new Remote(dbValue.id, dbValue.location, switches);
    }

    private static mapSwitch(dbValue: any): Switch {
        var flips: Flip[] = dbValue.flips.map(Data.mapFlips);
        return new Switch(dbValue.id, dbValue.state, dbValue.name, flips);
    }

    private static mapFlips(dbValue: any): Flip {
        return new Flip(dbValue.id, dbValue.direction, dbValue.hour, dbValue.minute, dbValue.timeOfDay);
    }
}