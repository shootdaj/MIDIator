import { Component, ViewChild, Injectable, Input, Output, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import '../rxjs-operators';
import { Subscription } from 'rxjs/Subscription';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption } from '../models/domainModel';
import { Subject } from 'rxjs/Subject';
import {SlimLoadingBarService} from 'ng2-slim-loading-bar';


@Injectable()
export class ProfileService {

    public profileChanges = new Subject<Profile>();

    constructor(private http: Http, private slimLoadingBarService: SlimLoadingBarService) {

    }

    getProfile() {
        this.slimLoadingBarService.start();
	    this.http.get("http://localhost:9000/midi/profile")
            .map(response => <Profile>response.json())
            .subscribe(data => {
	                this.profileChanges.next(data);
                    this.slimLoadingBarService.complete();
	            },
            err => console.log(err));
    }

    postProfile(profile: Profile) {
        this.slimLoadingBarService.start();
		this.http.post("http://localhost:9000/midi/profile", profile)
			.map(response => <Profile>response.json())
			.subscribe(data => {
		            this.profileChanges.next(data);
                    this.slimLoadingBarService.complete();
		        },
				err => console.log(err));
	}
}