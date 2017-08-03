
export class DbWorker {
    message(message: MessageEvent) {
        this.requestData(message.data);
    }

    requestData(path: string) {
        console.log('requestData', path);
        var xhr = new XMLHttpRequest();
        xhr.open('GET', path, true);
        xhr.addEventListener('readystatechange', this.requestCallback.bind(this));
        xhr.send();
    }

    requestCallback(event: Event): void {
        var xhr = <XMLHttpRequest>event.currentTarget;
        if (!xhr || !(xhr.readyState === xhr.DONE)) return;
        if (!xhr.responseText) return console.warn('request does not have a body')
        
        var response: any[];
        try {
            response = JSON.parse(xhr.responseText)
            console.log('responseText parsed', response)
        } catch (e) {
            return console.warn('responseText was only text')
        }
        this.startUpdate(response);
    }
    
    startUpdate(response: any): void {
        console.log('startUpdate');
        var request = indexedDB.open('RoboHome');
        request.addEventListener('upgradeneeded',this.createStore.bind(this, response));
        request.addEventListener('success', this.connected.bind(this, response));
        request.addEventListener('error', event => {
            console.error('requestError', event);
        })
    }

    connected(response: any[], event: Event) {
        console.log('connected')
        var req = <IDBRequest>(event.target);
        var db = <IDBDatabase>req.result;
        this.openStore(db, response);        
    }

    createStore(update: any[], event: Event): void {
        console.log('creating store');
        var parsed = this.parseUpdate(update);
        var response = <IDBOpenDBRequest>event.target;
        var db: IDBDatabase = response.result;
        var remoteStore = db.createObjectStore('remotes', {keyPath: 'id'});
        remoteStore.createIndex('id', 'id', {unique: true});
        remoteStore.transaction.addEventListener('complete', this.addValues.bind(this, db, 'remotes', parsed.remotes));
        var switchStore: IDBObjectStore = db.createObjectStore('switches', {keyPath: 'id'});
        switchStore.createIndex('id', 'id', {unique: true});
        switchStore.createIndex('remoteId', 'remoteId', {unique: false});
        switchStore.transaction.addEventListener('complete', this.addValues.bind(this, db, 'switches', parsed.switches))
        var flipsStore: IDBObjectStore = db.createObjectStore('flips', {keyPath: 'id'});
        flipsStore.createIndex('id', 'id', {unique: true});
        flipsStore.createIndex('switchId', 'switchId', {unique: false});
        flipsStore.transaction.addEventListener('complete', this.addValues.bind(this, db, 'flips', parsed.flips));
        event.preventDefault();
    }

    addValues(db: IDBDatabase, storeName: string, values: any[], event) {
        var store = db.transaction(storeName, 'readwrite').objectStore(storeName);
        for (var i = 0; i < values.length; i++) {
            store.add(values[i]);
        }
        // if (!value || value.length < 1) return;
        // var value = values.splice(0,1)[0];
        // var addReq = store.add(values[0]);
        // var nextValues = values.slice(1);
        // addReq.addEventListener('success', this.addValues.bind(this,store,values));
        // addReq.addEventListener('error', e => {throw new Error('Unable to add')});
    }

    parseUpdate(update: any[]) {
        var ret = {
            remotes: [],
            switches: [],
            flips: []
        };
        for (var i = 0; i < update.length; i++) {
            var remote = update[i];
            ret.remotes.push({
                id: remote.id,
                location: remote.location
            });
            for (var j = 0; j < remote.switches.length;j++) {
                var sw = remote.switches[j];
                ret.switches.push({
                    id: sw.id,
                    remoteId: remote.id,
                    name: sw.name,
                    state: sw.state,
                    onPin: sw.onPin,
                    offPin: sw.offPin
                });
                for (var z = 0; z < sw.flips.length; z++) {
                    var flip = sw.flips[z];
                    ret.flips.push({
                        id: flip.id,
                        switchId: sw.id,
                        direction: flip.direction,
                        hour: flip.hour,
                        minute: flip.minute,
                        timeOfDat: flip.timeOfDay
                    });
                }
            }
        }
        return ret;
    }

    openStore(db: IDBDatabase, update: any[]) {
        console.log('opening store');
        var parsedUpdate = this.parseUpdate(update);
        var remoteStore = db.transaction('remotes', 'readwrite').objectStore('remotes');
        remoteStore.openCursor().addEventListener('success', this.updateStore.bind(this, remoteStore, parsedUpdate.remotes));
        var switchStore = db.transaction('switches', 'readwrite').objectStore('switches');
        switchStore.openCursor().addEventListener('success', this.updateStore.bind(this, switchStore, parsedUpdate.switches));
        var flipStore = db.transaction('flips', 'readwrite').objectStore('flips');
        flipStore.openCursor().addEventListener('success', this.updateStore.bind(this, flipStore, parsedUpdate.flips));
    }

    updateStore(store: IDBObjectStore, update: any[], event) {
        console.log('cursor opened', store, update, event);
        var req = <IDBOpenDBRequest>(event.target);
        var cursor = <IDBCursorWithValue>req.result;
        if (cursor) {
            console.log('cursor not empty', cursor);
            var dbValue = cursor.value;
            console.log('key', cursor.key, 'value', dbValue);
            var foundIndex;
            var search = update.filter((item, i) => {
                if (item.id === cursor.key) {
                    if (!foundIndex) {
                        foundIndex = i;
                    }
                    return true;
                } else {
                    return false;
                }
            });
            var toBeUpdated = search[0];
            console.log('update value', toBeUpdated);
            if (!toBeUpdated) {
                cursor.delete();
                cursor.continue();
            } else {
                console.log('updating current value')
                var updateRequest = cursor.update(toBeUpdated);
                updateRequest.onsuccess = function(event: Event) {
                    console.log('update completed');
                    update.splice(foundIndex, 1);
                    cursor.continue();
                }
                updateRequest.onerror = function(event: Event) {
                    console.error('update failed', event);
                }
            }
        } else {
            console.log('cursor is empty');
            for (var i = 0; i < update.length; i++) {
                var remote = update[i];
                console.log('adding', remote);
                store.add(remote);
            }
        }
    }
}

var worker = new DbWorker();
onmessage = worker.message.bind(worker);