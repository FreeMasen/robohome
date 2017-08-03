import {Component, Inject, OnInit, ViewChild} from '@angular/core';
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
                private route: ActivatedRoute,
                ) {
    }

    ngOnInit(): void {
        this.route.params.forEach(params => {
            var id = params['id'];
            if (!isNaN(id)) {
                this.data.getRemote(id)
                .then(remote => this.addRemote(remote));
            } else {
                this.addRemote(new Remote());
            }
        });
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
        if (this.dirtyForm() && !this.validateRemote())
            return this.saved(false);
        this.data.saveRemote(this.remote)
        .then(success => this.saved(success))
        .catch(e => {throw e});
    }

    private saved(success): void {
        if (success) this._lastSaved = new Date();
        setTimeout(this.saveChanges.bind(this), 5000)
    }

    private dirtyForm(): boolean {
        var dirtyElements = document.querySelectorAll('.ng-dirty');
        var touchedElements = document.querySelectorAll('.ng-touched');
        return dirtyElements != undefined || touchedElements != undefined;
    }

    private validateRemote(): boolean {
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