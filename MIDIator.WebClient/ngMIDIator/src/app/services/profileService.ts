import { Component, ViewChild, Injectable, Input, Output, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { zip } from 'rxjs/Observable/zip';
import '../rxjs-operators';
import { Subscription } from 'rxjs/Subscription';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption } from '../models/domainModel';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Subject } from 'rxjs/Subject';
import {SlimLoadingBarService} from 'ng2-slim-loading-bar';
import {FormService} from "./formService";
import {MIDIService} from "./midiService";
declare var componentHandler;


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
			.subscribe(profileData => {
				let sub = this.midiService.staticDataSubject.subscribe(data => {
					this.formService.setForm(profileData);
					this.slimLoadingBarService.complete();
					sub.unsubscribe();
				});
				this.midiService.getStaticData();
			},
            err => console.log(err));
    }

    private postProfile(profile: Profile, refreshMIDIOutputDevice: boolean) {
        this.slimLoadingBarService.start();
		this.http.post(this.profileURL, profile)
			.map(response => <Profile>response.json())
			.subscribe(profileData => {
				if (refreshMIDIOutputDevice) {
					var loaderSub = this.midiService.availableOutputDevicesChanges.subscribe(devices => {
						let sub = this.midiService.staticDataSubject.subscribe(data => {
							this.formService.setForm(profileData);
							this.slimLoadingBarService.complete();
							sub.unsubscribe();
							loaderSub.unsubscribe();
							//setTimeout(() => {
							//	componentHandler.upgradeAllRegistered();
							//});
						});
						this.midiService.getStaticData();
					});
					this.midiService.getAvailableOutputDevices();
				} else {
					let sub = this.midiService.staticDataSubject.subscribe(data => {
						this.formService.setForm(profileData);
						this.slimLoadingBarService.complete();
						sub.unsubscribe();
					});
					this.midiService.getStaticData();
				}
			},
			err => console.log(err));
	}
}