import {Injectable} from '@angular/core';
import {Http} from '@angular/http';

import 'rxjs';

import {Remote, Switch, Flip, SwitchState, Time} from '../models';
import KeyTimes from '../models/keyTimes';

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
                            return responseBody.map(Remote.fromJson);
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
                            return Remote.fromJson(res.json());
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
                                return Remote.fromJson(
                                    json
                                );
                            }
                        })
                        .catch(e => {
                            console.error('saveRemote.response.error', e);
                            throw e;
                        });
    }

    private static mapRemote(dbValue: any): Remote {
        return Remote.fromJson(dbValue);
    }

    private listenForChanges(): void {
        this.worker = new Worker('/js/worker.js');
        this.worker.addEventListener('message', this.workerCallback.bind(this));
        this.worker.postMessage('/api/remotes');
    }

    private workerCallback(worker, event) {
        console.log('workerCallback', worker ,event);
    }

    async getDawn(): Promise<Time> {
        return await this.getKeyTimes().then(k => k.dawn);
    }

    async getSunset(): Promise<Time> {
        return await this.getKeyTimes().then(k => k.sunset);
    }

    async getDusk(): Promise<Time> {
        return await this.getKeyTimes().then(k => k.dusk);
    }

    private async getKeyTimes(): Promise<KeyTimes> {
        let text = localStorage.getItem('key-times');
        let ret: KeyTimes;
        if (!text) {
            ret = await this.fetchKeyTimes();
            this.storeKeyTimes(ret);
            return ret;
        }
        let partial = JSON.parse(text);
        ret = KeyTimes.fromJSON(partial);
        if (ret.isOutOfDate()) {
            ret = await this.fetchKeyTimes();
            this.storeKeyTimes(ret);
        }
        return ret;
    }

    private async fetchKeyTimes(): Promise<KeyTimes> {
        return this.http.get('/api/keyTimes')
        .toPromise()
        .then(res => res.json())
        .then(json => KeyTimes.fromJSON(json));
    }

    private storeKeyTimes(times: KeyTimes) {
        let text = JSON.stringify(times);
        localStorage.setItem('key-times', text);
    }
}