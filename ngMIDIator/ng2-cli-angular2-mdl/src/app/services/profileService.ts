import { Component, ViewChild, Injectable, Input, Output, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import '../rxjs-operators';
import { Subscription } from 'rxjs/Subscription';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption } from '../models/domainModel';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Subject } from 'rxjs/Subject';
import {SlimLoadingBarService} from 'ng2-slim-loading-bar';
import {FormService} from "./formService";


@Injectable()
export class ProfileService {

	private profileURL: string = "http://localhost:9000/midi/profile";

	private innerProfile: Profile;
	
	public profileChanges = new Subject<Profile>();
	private subscriptions: { [name: string]: Subscription; } = {};

    constructor(private http: Http,
		private slimLoadingBarService: SlimLoadingBarService,
		private formService: FormService) {
	    this.subscriptions['profileChanges'] = (this.profileChanges
            .subscribe(data => {
                this.innerProfile = data;
				this.formService.setForm(this.innerProfile);
            }));
    }

	get profile(): Profile {
		return this.innerProfile;
	}


	public loadProfile() {
		this.getProfileFromServer();
	}

	public saveProfile() {
		let profile = this.formService.getForm().value;
		let valid = this.formService.getForm().valid;

		console.log(profile, valid);
		if (valid)
            this.postProfile(profile);
    }

    private getProfileFromServer() {
        this.slimLoadingBarService.start();
		this.http.get(this.profileURL)
            .map(response => <Profile>response.json())
            .subscribe(data => {
				this.profileChanges.next(data);
				this.slimLoadingBarService.complete();
			},
            err => console.log(err));
    }

    private postProfile(profile: Profile) {
        this.slimLoadingBarService.start();
		this.http.post(this.profileURL, profile)
			.map(response => <Profile>response.json())
			.subscribe(data => {
				this.profileChanges.next(data);
				this.slimLoadingBarService.complete();
			},
			err => console.log(err));
	}
}