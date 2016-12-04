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
import {MIDIService} from "./midiService";


@Injectable()
export class ProfileService {

	private profileURL: string = "http://localhost:9000/midi/profile";
	private subscriptions: { [name: string]: Subscription; } = {};

    constructor(private http: Http,
		private slimLoadingBarService: SlimLoadingBarService,
		private formService: FormService,
		private midiService: MIDIService) {
    }

	public loadProfile() {
		this.getProfileFromServer();
	}

	public saveProfile(refreshMIDIOutputDevice?: boolean) {
		let profile = this.formService.getForm().value;
		let valid = this.formService.getForm().valid;

		console.log(profile, valid);
		if (valid)
            this.postProfile(profile, refreshMIDIOutputDevice);
    }
	
	private getProfileFromServer() {
        this.slimLoadingBarService.start();
		this.http.get(this.profileURL)
            .map(response => <Profile>response.json())
            .subscribe(data => {
				this.formService.setForm(data);
				this.slimLoadingBarService.complete();
			},
            err => console.log(err));
    }

    private postProfile(profile: Profile, refreshMIDIOutputDevice: boolean) {
        this.slimLoadingBarService.start();
		this.http.post(this.profileURL, profile)
			.map(response => <Profile>response.json())
			.subscribe(data => {
					if (refreshMIDIOutputDevice) {
						var loaderSub = this.midiService.availableOutputDevicesChanges.subscribe(devices => {
							this.formService.setForm(data);
							this.slimLoadingBarService.complete();
							loaderSub.unsubscribe();
						});
						this.midiService.getAvailableOutputDevices();
					} else {
						this.formService.setForm(data);
						this.slimLoadingBarService.complete();
					}
			},
			err => console.log(err));
	}
}