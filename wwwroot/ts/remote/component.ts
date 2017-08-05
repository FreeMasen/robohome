import {Component, Inject, OnInit} from '@angular/core';
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
                return this.addRemote(new Remote());
            }
            this.data.getRemote(id)
            .then(remote => this.addRemote(remote));
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
        this.saved(true);
    }

    get lastSaved(): string {
        if (!this._lastSaved) return '';
        var now = new Date();
        var secondsAgo = Math.floor((now.getTime() - this._lastSaved.getTime()) / 1000)
        return secondsAgo + ' seconds ago';
    }

    private saveChanges(): void {
        if (!this.isValid())
            return this.saved(false, true);
        this.data.saveRemote(this.remote)
        .then(success => this.saved(success))
        .catch(e => {throw e});
    }

    private saved(success, invalid: boolean = false): void {
        if (success) {
            this._lastSaved = new Date();
        } else {
            if (!invalid) {
                setTimeout(this.saveChanges.bind(this), 5000);
            }
        }
    }

    private dirtyForm(): boolean {
        var dirtyElements = document.querySelectorAll('.ng-dirty');
        var touchedElements = document.querySelectorAll('.ng-touched');
        console.log('dirtyForm()', dirtyElements, touchedElements);
        return dirtyElements && dirtyElements.length > 0 || touchedElements && touchedElements.length > 0;
    }

    private isValid(): boolean {
        var ret = this.remote.location != undefined &&
                this.validateSwitches(...this.remote.switches);
        return ret;
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
            if (flip.hour < 1 || flip.hour > 12) {
                console.warn('invalid hour for flip', flip)
                return false;
            }
            if (flip.minute < 0 || flip.minute > 59) {
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