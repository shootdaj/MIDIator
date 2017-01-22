import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http, Headers, RequestOptions } from '@angular/http';
import { zip } from 'rxjs/Observable/zip';
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
		this.availableChannelCommandsSubject.subscribe((data) => {
			this.availableChannelCommands = data;
				console.log("setting midiService.availableChannelCommands");
			}
		);
		this.availableMIDIChannelsSubject.subscribe((data) => this.availableMIDIChannels = data);
		this.availableInputMatchFunctionsSubject.subscribe((data) => this.availableInputMatchFunctions = data);
		this.availableTranslationFunctionsSubject.subscribe((data) => this.availableTranslationFunctions = data);

	}


	availableInputDevicesChanges: Subject<MIDIInputDevice[]> = new Subject<MIDIInputDevice[]>();
	availableOutputDevicesChanges: Subject<MIDIOutputDevice[]> = new Subject<MIDIOutputDevice[]>();
	availableChannelCommandsSubject: Subject<ChannelCommand[]> = new Subject<ChannelCommand[]>();
	public availableChannelCommands: ChannelCommand[] = null;
	availableMIDIChannelsSubject: Subject<number[]> = new Subject<number[]>();
	public availableMIDIChannels: number[] = null;
	availableInputMatchFunctionsSubject: Subject<InputMatchFunction[]> = new Subject<InputMatchFunction[]>();
	public availableInputMatchFunctions: InputMatchFunction[] = null;
	availableTranslationFunctionsSubject: Subject<TranslationFunction[]> = new Subject<TranslationFunction[]>();
	public availableTranslationFunctions: TranslationFunction[] = null;
	staticDataSubject: Subject<any> = new Subject<any>();

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
		console.log("executing getAvailableChannelCommands()");
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

	getStaticData() {
		if (this.availableChannelCommands === null ||
			this.availableMIDIChannels === null ||
			this.availableInputMatchFunctions === null ||
			this.availableTranslationFunctions === null) {

			let sub = zip(this.availableChannelCommandsSubject,
					this.availableMIDIChannelsSubject,
					this.availableInputMatchFunctionsSubject,
					this.availableTranslationFunctionsSubject)
				.subscribe(data => {
					this.staticDataSubject.next(data);
					sub.unsubscribe();
				});

			this.getAvailableChannelCommands();
			this.getAvailableMIDIChannels();
			this.getAvailableInputMatchFunctions();
			this.getAvailableTranslationFunctions();
		} else {
			this.staticDataSubject.next(null);
		}
	}

    startMIDIReader(inputDeviceName: string) {
        this.sendSignalRMessage("http://localhost:9000/midi/StartMIDIReader", { inputDeviceName });
    }

    stopMIDIReader(inputDeviceName: string) {
        this.sendSignalRMessage("http://localhost:9000/midi/StopMIDIReader", { inputDeviceName });
    }

    sendMessageToOutputDevice(message: ChannelMessage, outputDeviceName: string) {
        this.sendSignalRMessage("http://localhost:9000/midi/SendMessageToOutputDevice", { message, outputDeviceName });
    }

    sendSignalRMessage(endpoint: string, content: any) {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post(endpoint, content, options).subscribe(data => { },
            err => console.log(err));
    }

}