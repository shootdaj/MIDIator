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
export class ProfileService {

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
}