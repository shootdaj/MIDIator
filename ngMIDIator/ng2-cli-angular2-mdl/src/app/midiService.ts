import { ITranslation } from './base';
import { ITranslationMap } from './base';
import { MIDIInputDevice } from './base';
import { MIDIOutputDevice } from './base';
import { Translation } from './base';
import { ShortMessage } from './base';
import { InputMatchFunction } from './base';
import { TranslationFunction } from './base';
import { MessageType } from './base';
import { Profile } from './base';
import { Transformation } from './base';

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


}