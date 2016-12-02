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

@Injectable()
export class RealtimeService {

	private realtime: boolean;
    private subscriptions: { [name: string]: Subscription; };
	private debounceTimeInMS: number = 1000;

	constructor(private profileService: ProfileService) {
		
	}

	public isRealtime() : boolean{
		return this.realtime;
	}

	public disableRealtime() {
		console.log('disabling realtime');
        this.detachRealtime();
		this.realtime = false;
    }

    public enableRealtime(form: FormGroup) {
		console.log('enabling realtime');
		this.attachRealtimeToForm(form);
		this.realtime = true;
    }

	public attachRealtimeToForm(form: FormGroup) {
		this.subscriptions['formValueChanges'] = (form.valueChanges.debounceTime(this.debounceTimeInMS)
            .subscribe(values => {
				if (form.valid)
					this.profileService.postProfile(<Profile>values);
			}));
	}

	public handleRealtimeForForm(form: FormGroup) {
		if (this.realtime) {
			this.attachRealtimeToForm(form);
		}
	}

	public detachRealtime() {
		this.subscriptions['formValueChanges'].unsubscribe();
        this.subscriptions['formValueChanges'] = null;
	}

	

	//availableInputDevicesChanges: Subject<MIDIInputDevice[]> = new Subject<MIDIInputDevice[]>();
	//getAvailableInputDevices() {
	//	this.http.get('http://localhost:9000/midi/AvailableInputDevices')
 //           .map(response => <MIDIInputDevice[]>response.json())
	//		.subscribe(data => this.availableInputDevicesChanges.next(data),
	//		err => console.log(err));
	//}
}