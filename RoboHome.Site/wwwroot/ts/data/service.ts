import {Injectable} from '@angular/core';
import {Http} from '@angular/http';

import 'rxjs';

import {Remote, Switch, Flip, SwitchState} from '../models';

@Injectable()
export class Data {
    private worker: Worker;
    remotes: Remote[];
    constructor(private http: Http) {
    }

    flip(switchId: number, newState: SwitchState): Promise<boolean> {
        console.log('flip', switchId, newState);
        var url = `/api/flip?switchId=${switchId}&newState=${newState}`;
        return this.http
                    .put(url, null)
                    .toPromise()
                    .then(response => {
                        return response.status == 200
                    })
                    .catch(err => {
                        console.error(err);
                        return false;
                    });
    }

    deleteRemote(remote: Remote): Promise<boolean> {
        console.log('deleteRemote', remote);
        return this.http.delete(`/api/delete/remote/${remote.id}`)
            .toPromise()
            .then(res => {
                if (res.ok) return true;
                return false;
            })
            .catch(e => {
                throw e;
            });
    }

    getRemotes(): Promise<Remote[]> {
        console.log('getRemotes');
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

    getRemote(id?: string): Promise<Remote> {
        console.log('getRemote');
        var path = '/api/remote'
        if (id) path += '/' + id;
        return this.http.get(path)
                        .toPromise()
                        .then(res => {
                            return Data.mapRemote(res.json());
                        })
                        .catch(e => {
                            throw e;
                        });
    }

    saveRemote(remote: Remote): Promise<Remote> {
        console.log('saveRemote', remote);
        return this.http.post('/api/remote', remote)
                        .toPromise()
                        .then(res => {
                            console.log('saveRemote.response', res);
                            if (res.ok) {
                                let json = res.json();
                                return new Remote(
                                    json.id,
                                    json.location,
                                    json.switches
                                );
                            }
                        })
                        .catch(e => {
                            console.error('saveRemote.response.error', e);
                            throw e;
                        });
    }

    private static mapRemote(dbValue: any): Remote {
        var switches: Switch[] = dbValue.switches.map(Data.mapSwitch);
        return new Remote(dbValue.id, dbValue.location, switches);
    }

    private static mapSwitch(dbValue: any): Switch {
        var flips: Flip[] = dbValue.flips.map(Data.mapFlips);
        return new Switch(dbValue.id, dbValue.state, dbValue.name, flips, dbValue.onPin, dbValue.offPin);
    }

    private static mapFlips(dbValue: any): Flip {
        return new Flip(dbValue.id, dbValue.direction, dbValue.hour, dbValue.minute, dbValue.timeOfDay);
    }

    private listenForChanges(): void {
        this.worker = new Worker('/js/worker.js');
        this.worker.addEventListener('message', this.workerCallback.bind(this));
        this.worker.postMessage('/api/remotes');
    }

    private workerCallback(worker, event) {
        console.log('workerCallback', worker ,event);
    }
}