import {Component, Inject, OnInit} from '@angular/core';
import {Location} from '@angular/common';
import {ActivatedRoute} from '@angular/router';
import {Form} from '@angular/forms';
import {Remote} from '../models';
import {Data} from '../services';

@Component({
    selector: 'remote',
    templateUrl: '../../templates/remote.html',
    styleUrls: ['../../css/remote.css']
})
export class RemoteEditor implements OnInit {
    private id: number;

    constructor(private data: Data,
                private location: Location,
                private route: ActivatedRoute) {
        this.route.params.forEach(params => {
            var id = params['id'];
            
        });
    }
    ngOnInit(): void {
        this.data.getRemotes()
    }
}