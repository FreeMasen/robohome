import {Component, Inject, OnInit, Output, EventEmitter} from '@angular/core';
import {Location} from '@angular/common';
import {ActivatedRoute} from '@angular/router';
import {Form} from '@angular/forms';
import {Remote, Switch, Flip, SwitchState} from '../models';
import {Data} from '../services';

@Component({
    selector: 'remote-editor',
    templateUrl: '../../templates/remote.html',
    styleUrls: ['../../css/remote.css']
})
export class RemoteEditor implements OnInit {
    private id: number;
    private remote: Remote;
    private _lastSaved: Date;

    constructor(private data: Data,
                private location: Location,
                private route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.route.params.forEach(params => {
            var id = params['id'];
            if (isNaN(id)) {
                return this.newRemote();
            }
            this.data.getRemote(id)
                .then(remote => this.addRemote(remote));
        });
    }

    newRemote() {
        let forSending = new Remote();
        this.data.saveRemote(forSending)
            .then(remote => {
                //this will have the new ID assigned
                this.addRemote(remote);
            });
    }

    remoteChange(event): void {
        console.log('remoteChange', event);
        this.saveChanges();
    }

    switchChange(event): void {
        console.log('RemoteEditor.switchChange', event);
        this.saveChanges();
    }

    childDeleted(child): void {
        console.log('RemoteEditor.childDeleted', event);
        if (child instanceof Switch) {
            var index = this.remote.switches.indexOf(child);
            if (index > -1) {
                this.remote.switches.splice(index, 1);
            }
        }
        this.saveChanges();
    }

    addRemote(remote:Remote): void {
        this.remote = remote;
        // this.saved(true);
    }

    get lastSaved(): string {
        if (!this._lastSaved) return '';
        var now = new Date();
        var secondsAgo = Math.floor((now.getTime() - this._lastSaved.getTime()) / 1000)
        return secondsAgo + ' seconds ago';
    }

    addSwitch(): void {
        this.remote.switches.push(new Switch());
    }

    private saveChanges(): void {
        if (!this.isValid())
            return this.saved(null, true);
        this.data.saveRemote(this.remote)
        .then(remote => this.saved(remote))
        .catch(e => {throw e});
    }

    private saved(newRemote?: Remote, invalid: boolean = false): void {
        if (newRemote) {
            this.remote = newRemote;
            this._lastSaved = new Date();
        } else {
            if (!invalid) {
                setTimeout(this.saveChanges.bind(this), 5000);
            }
        }
    }

    private isValid(): boolean {
        if (!this.remote.location || this.remote.location == '') {
            console.warn('Invalid switch location', this.remote);
            return false;
        }
        return this.validateSwitches(...this.remote.switches);
    }

    private validateSwitches(...switches: Switch[]): boolean {
        for (var i = 0; i < switches.length; i++) {
            var sw = switches[i];
            if (sw.name == undefined) {
                console.warn('invalid switch name', sw);
                return false;
            }
            if (isNaN(sw.offPin)) {
                console.warn('switch offpin is NaN', sw);
                return false;
            }
            if (isNaN(sw.onPin)) {
                console.warn('switch onpin is NaN', sw);
                return false;
            }
            if (!this.validateFlips(sw.flips)) {
                return false;
            }
        }
        return true;
    }

    private validateFlips(flips: Flip[]): boolean {
        for (var i = 0; i < flips.length;i++) {
            var flip = flips[i];
            if (flip.time.hour < 1 || flip.time.hour > 12) {
                console.warn('invalid hour for flip', flip)
                return false;
            }
            if (flip.time.minute < 0 || flip.time.minute > 59) {
                console.warn('invalid minute for flip', flip)
                return false;
            }
            if (flip.direction !== SwitchState.On &&
                flip.direction !== SwitchState.Off) {
                console.warn('invalid direction for flip', flip)
                return false;
            }
        }
        return true;
    }
}