import { Component, ViewChild, Injectable, Input, Output, EventEmitter, OnInit, AfterViewInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../../rxjs-operators';
import { EnumValues } from 'enum-values';
import { MIDIService } from '../../services/midiService';
import { ProfileService } from '../../services/profileService';
import { DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { DropdownOption } from '../../components/mdl-dropdown/dropdownOption';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, TranslationMap } from '../../models/domainModel';
import { TransformationComponent } from '../../components/transformation/transformation.component';
import { TranslationComponent } from '../../components/translation/translation.component';
import { TextInputComponent } from '../../components/mdl-textinput/mdl-textinput.component';

@Component({
    selector: 'profile',
    templateUrl: './profile.component.html'
})

export class ProfileComponent {

	@Input() form: FormGroup;

	@Input() realtime: Boolean;
	@Output() realtimeChange = new EventEmitter<Boolean>();

	private get switchRealtime(): Boolean {
		return this.realtime;
	}

	private set switchRealtime(inValue: Boolean) {
		this.realtime = inValue;
		this.realtimeChange.emit(inValue);
	}

	enableRealtime() {
		this.switchRealtime = true;
	}

	disableRealtime() {
		this.switchRealtime = false;
	}

	constructor() {
    }
}