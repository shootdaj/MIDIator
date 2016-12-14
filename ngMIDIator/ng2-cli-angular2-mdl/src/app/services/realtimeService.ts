import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../rxjs-operators';
import { EnumValues } from 'enum-values';
import { ProfileService } from '../services/profileService';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../models/domainModel';
import {FormService} from "./formService";

@Injectable()
export class RealtimeService {

	private realtime: boolean = true;
    private subscriptions: { [name: string]: Subscription; } = {};
	private debounceTimeInMS: number = 1000;

	constructor(private profileService: ProfileService, private formService: FormService) {
		//this.attachFormChanges();
	}

	public attachFormChanges() {
		this.formService.formChanges.subscribe(form =>
			this.handleRealtimeForForm(form)
		);
	}

	public handleRealtimeForForm(form: FormGroup) {
		if (this.realtime) {
			this.attachRealtimeToForm(form);
		}
	}

	public isRealtimeEnabled(): boolean {
		return this.realtime;
	}

	public enableRealtime() {
		if (!this.realtime) {
			console.log('enabling realtime');
			this.attachRealtimeToForm(this.formService.getForm());
			this.realtime = true;
		}
	}

	public disableRealtime() {
		if (this.realtime) {
			console.log('disabling realtime');
			this.detachRealtime();
			this.realtime = false;
		}
	}

	private attachRealtimeToForm(form: FormGroup) {
		if (this.subscriptions['formValueChanges'] != null) {
			this.detachRealtime();
		}

		this.subscriptions['formValueChanges'] = (form.valueChanges.debounceTime(this.debounceTimeInMS)
			.subscribe(values => { // values is ignored because saveProfile() is implicitly tied to formService.getForm().value
				this.profileService.saveProfile(true);
			}));

	}

	private detachRealtime() {
		this.subscriptions['formValueChanges'].unsubscribe();
        this.subscriptions['formValueChanges'] = null;
	}
}