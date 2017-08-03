import {Injectable} from '@angular/core';
import {Http} from '@angular/http';

import 'rxjs';

import {Remote, Switch, Flip} from '../models';

@Injectable()
export class Data {
    private worker: Worker;
    remotes: Remote[];
    constructor(private http: Http) {
        console.log('new Data()');
    }

    getRemotes(): Promise<Remote[]> {
        console.log('Data.getRemotes()');
        var openReq = indexedDB.open('RoboHome');
        openReq.addEventListener('success', this.dbOpened.bind(this));
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

    dbOpened(event: Event):void {
        var request = <IDBOpenDBRequest>event.target;
        var db = <IDBDatabase>request.result;
        var transaction = db.transaction(['remotes', 'switches', 'flips']);
        var remoteStore = transaction.objectStore('remotes');
        remoteStore.get('id')
        
    }

    buildRemote(transaction, store, event): void {
        var request = <IDBRequest>event.target;
        var cursor = <IDBCursorWithValue>request.result;
        var switchStore: IDBObjectStore = transaction.objectStore('switches');
        if (cursor) {
            var getRequest = switchStore.index('remoteId').get(cursor.value.id);
            getRequest.addEventListener('success', this.buildSwitch.bind(this, transaction, store));
        }
    }

    buildSwitch(transaction, store, event): void {

    }

    getRemote(id?: string): Promise<Remote> {
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

    saveRemote(remote: Remote): Promise<boolean> {
        return this.http.post('/api/remote', remote)
                        .toPromise()
                        .then(res => {
                            if (res.ok)
                                return true;
                            return false;
                        })
                        .catch(e => {
                            throw e;
                        })
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