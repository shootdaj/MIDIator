import { MIDIInputDevice } from './base';
import { MIDIOutputDevice } from './base';
import { Translation } from './base';
import { ShortMessage } from './base';
import { MessageType } from './base';
import { Profile } from './base';
import { Transformation, ChannelCommand, TranslationFunction, InputMatchFunction } from './base';
import { DropdownOption } from './mdl-dropdown.component';

import {Injectable} from '@angular/core';
import { Http } from '@angular/http';

import { Subject } from 'rxjs/Subject';

@Injectable()
export class MIDIService {

	private profileUrl: string = "http://localhost:9000/midi/profile";

	constructor(private http: Http) {

	}

	availableInputDevicesSubject: Subject<MIDIInputDevice[]> = new Subject<MIDIInputDevice[]>();
	availableOutputDevicesSubject: Subject<MIDIOutputDevice[]> = new Subject<MIDIOutputDevice[]>();
	availableChannelCommandsSubject: Subject<ChannelCommand[]> = new Subject<ChannelCommand[]>();
	availableMIDIChannelsSubject: Subject<number[]> = new Subject<number[]>();
	availableInputMatchFunctionsSubject: Subject<InputMatchFunction[]> = new Subject<InputMatchFunction[]>();
	availableTranslationFunctionsSubject: Subject<TranslationFunction[]> = new Subject<TranslationFunction[]>();

	getAvailableInputDevices() {
		this.http.get('http://localhost:9000/midi/AvailableInputDevices')
			.map(response => response.json())
			.subscribe(data => this.availableInputDevicesSubject.next(data),
			err => console.log(err));
	}

	getAvailableOutputDevices() {
		this.http.get('http://localhost:9000/midi/AvailableOutputDevices')
			.map(response => response.json())
			.subscribe(data => this.availableOutputDevicesSubject.next(data),
			err => console.log(err));
	}

	getAvailableChannelCommands() {
		this.http.get('http://localhost:9000/midi/AvailableChannelCommands')
			.map(response => response.json())
			.subscribe(data => this.availableChannelCommandsSubject.next(data),
			err => console.log(err));
	}

	getAvailableMIDIChannels() {
		this.http.get('http://localhost:9000/midi/AvailableMIDIChannels')
			.map(response => response.json())
			.subscribe(data => this.availableMIDIChannelsSubject.next(data),
			err => console.log(err));
	}

	getAvailableInputMatchFunctions() {
		this.http.get('http://localhost:9000/midi/AvailableInputMatchFunctions')
			.map(response => response.json())
			.subscribe(data => this.availableInputMatchFunctionsSubject.next(data),
			err => console.log(err));
	}

	getAvailableTranslationFunctions() {
		this.http.get('http://localhost:9000/midi/AvailableTranslationFunctions')
			.map(response => response.json())
			.subscribe(data => this.availableTranslationFunctionsSubject.next(data),
			err => console.log(err));
	}
}