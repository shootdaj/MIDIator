import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../rxjs-operators';
import { EnumValues } from 'enum-values';
import { ProfileService } from '../services/profileService';
import { IDropdownOption, IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../models/domainModel';
import { DropdownOption } from '../components/mdl-dropdown/dropdownOption';
import * as $ from 'jquery';

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

	deepRemove(obj, name) {
		delete obj[name];
		Object.keys(obj).forEach(key => {
			if (obj[key] instanceof Object)
				this.deepRemove(obj[key], name);
		});
	};

	maskCastProfile(rawProfile): Profile {
		var profile = new Profile();

		this.deepRemove(rawProfile, "label");
		this.deepRemove(rawProfile, "value");

		profile.name = rawProfile.name;
		rawProfile.transformations.forEach(rawXForm => {
			var xform = new Transformation();

			xform.inputDevice = new MIDIInputDevice();
			$.extend(xform.inputDevice, rawXForm.inputDevice);

			xform.outputDevice = new MIDIOutputDevice();
			$.extend(xform.outputDevice, rawXForm.outputDevice);

			profile.transformations.push(xform);
		});

		return profile;
	}

	getDropdownOption(input: any): DropdownOption {
		return new DropdownOption(input.name, input.name);
	}

	dropdownOptionValueSetFunction(inValue: any, options: IDropdownOption[], control: FormGroup): any {
		control.setValue(options.filter(x => x.value === inValue)[0]);
	}

	dropdownOptionValueGetFunction(control: FormGroup): any {
		return control.value.value;
	}

	imfValueSetFunction(inValue: any, options: IDropdownOption[], control: FormGroup): any {
		control.setValue(inValue);
	}

	imfValueGetFunction(control: FormGroup, options: IDropdownOption[]): any {
		return control.value;
	}

	tfValueSetFunction(inValue: any, options: IDropdownOption[], control: FormGroup): any {
		control.setValue(inValue);
	}

	tfValueGetFunction(control: FormGroup, options: IDropdownOption[]): any {
		return control.value;
	}
}