import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../rxjs-operators';
import { EnumValues } from 'enum-values';
import { ProfileService } from '../services/profileService';
import { DropdownOption, DropdownComponent } from '../components/mdl-dropdown/mdl-dropdown.component';
import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../models/domainModel';

@Injectable()
export class HelperService {
	constructor(private http: Http) {
		
	}

	maskCast(rawObj, constructor) {
		var obj = new constructor();
		for (var i in rawObj)
			obj[i] = rawObj[i];
		return obj;
	}

	//toFormGroup(profile: Profile) {
	//	let group: any = {};

		

	//	//questions.forEach(question => {
	//	//	group[question.key] = question.required ? new FormControl(question.value || '', Validators.required)
	//	//		: new FormControl(question.value || '');
	//	//});
	//	return new FormGroup(group);
	//}
}