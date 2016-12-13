﻿import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../rxjs-operators';
import { EnumValues } from 'enum-values';
import { ProfileService } from '../services/profileService';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../models/domainModel';

@Injectable()
export class MIDIService {

	constructor(private http: Http) {
	}

	availableInputDevicesChanges: Subject<MIDIInputDevice[]> = new Subject<MIDIInputDevice[]>();
	availableOutputDevicesChanges: Subject<MIDIOutputDevice[]> = new Subject<MIDIOutputDevice[]>();
	availableChannelCommandsSubject: Subject<ChannelCommand[]> = new Subject<ChannelCommand[]>();
	availableMIDIChannelsSubject: Subject<number[]> = new Subject<number[]>();
	availableInputMatchFunctionsSubject: Subject<InputMatchFunction[]> = new Subject<InputMatchFunction[]>();
	availableTranslationFunctionsSubject: Subject<TranslationFunction[]> = new Subject<TranslationFunction[]>();

	getAvailableInputDevices() {
		this.http.get('http://localhost:9000/midi/AvailableInputDevices')
            .map(response => <MIDIInputDevice[]>response.json())
			.subscribe(data => this.availableInputDevicesChanges.next(data),
			err => console.log(err));
	}

	getAvailableOutputDevices() {
		this.http.get('http://localhost:9000/midi/AvailableOutputDevices')
			.map(response => <MIDIOutputDevice[]>response.json())
			.subscribe(data => this.availableOutputDevicesChanges.next(data),
			err => console.log(err));
	}

	getAvailableChannelCommands() {
		this.http.get('http://localhost:9000/midi/AvailableChannelCommands')
			.map(response => <ChannelCommand[]>response.json())
			.subscribe(data => this.availableChannelCommandsSubject.next(data),
			err => console.log(err));
	}

	getAvailableMIDIChannels() {
		this.http.get('http://localhost:9000/midi/AvailableMIDIChannels')
			.map(response => <number[]>response.json())
			.subscribe(data => this.availableMIDIChannelsSubject.next(data),
			err => console.log(err));
	}

	getAvailableInputMatchFunctions() {
		this.http.get('http://localhost:9000/midi/AvailableInputMatchFunctions')
			.map(response => <InputMatchFunction[]>response.json())
			.subscribe(data => this.availableInputMatchFunctionsSubject.next(data),
			err => console.log(err));
	}

	getAvailableTranslationFunctions() {
		this.http.get('http://localhost:9000/midi/AvailableTranslationFunctions')
			.map(response => <TranslationFunction[]>response.json())
			.subscribe(data => this.availableTranslationFunctionsSubject.next(data),
			err => console.log(err));
	}

    startMIDIReader(inputDeviceName: string) {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post("http://localhost:9000/midi/StartMIDIReader",  { inputDeviceName }, options).subscribe(data => {},
            err => console.log(err));
    }

    stopMIDIReader(inputDeviceName: string) {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post("http://localhost:9000/midi/StopMIDIReader", { inputDeviceName }, options).subscribe(data => { },
            err => console.log(err));
    }
}