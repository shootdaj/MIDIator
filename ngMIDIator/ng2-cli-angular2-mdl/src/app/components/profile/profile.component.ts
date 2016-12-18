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
import { RealtimeService } from '../../services/realtimeService';
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

export class ProfileComponent implements AfterViewInit {

	@Input() form: FormGroup;

	constructor(private realtimeService: RealtimeService,
				private midiService: MIDIService) {
    }

	private isRealtimeEnabled(): boolean {
		return this.realtimeService.isRealtimeEnabled();
	}

	enableRealtime() {
		this.realtimeService.enableRealtime();
	}

	disableRealtime() {
		this.realtimeService.disableRealtime();
	}

	ngAfterViewInit(): void {
		
	}
}