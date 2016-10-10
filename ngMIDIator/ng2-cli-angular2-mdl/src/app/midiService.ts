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

@Injectable()
export class MIDIService {

	private profileUrl: string = "http://localhost:9000/midi/profile";

	constructor(private http: Http) {

	}

	getProfile(name: string, callback: (data: Profile) => void) {
		this.http.get(this.profileUrl)
			.map(response => response.json())
			.subscribe(data => {
				callback(<Profile>data);
				console.log(data);
			},
			err => console.log(err));
	}

	getAvailableInputDevices(callback: (data: MIDIInputDevice[]) => void) {
		this.http.get('http://localhost:9000/midi/AvailableInputDevices')
			.map(response => response.json())
			.subscribe(data => callback(<MIDIInputDevice[]>data),
			err => console.log(err));
	}

	getAvailableOutputDevices(callback: (data: MIDIOutputDevice[]) => void) {
		this.http.get('http://localhost:9000/midi/AvailableOutputDevices')
			.map(response => response.json())
			.subscribe(data => callback(<MIDIOutputDevice[]>data),
			err => console.log(err));
	}

	getAvailableChannelCommands(callback: (data: ChannelCommand[]) => void) {
		this.http.get('http://localhost:9000/midi/ChannelCommands')
			.map(response => response.json())
			.subscribe(data => callback(<ChannelCommand[]>data),
			err => console.log(err));
	}

	getAvailableMIDIChannels(callback: (data: number[]) => void) {
		this.http.get('http://localhost:9000/midi/MIDIChannels')
			.map(response => response.json())
			.subscribe(data => callback(<number[]>data),
			err => console.log(err));
	}

	//getAvailableChannelCommands(): DropdownOption[] {
	//	return Object.keys(ChannelCommand).map(key => new DropdownOption(key, ChannelCommand[key]));
	//}

	//getAvailableInputMatchFunctions(): DropdownOption[] {
	//	return Object.keys(InputMatchFunction).map(key => new DropdownOption(key, InputMatchFunction[key]));
	//}

	//getAvailableTranslationFunctions(): DropdownOption[] {
	//	return Object.keys(TranslationFunction).map(key => new DropdownOption(key, TranslationFunction[key]));
	//}
}