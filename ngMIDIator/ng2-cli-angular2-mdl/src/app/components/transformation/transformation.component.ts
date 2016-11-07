import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck, OnInit, OnDestroy } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../../rxjs-operators';
import { EnumValues } from 'enum-values';
import { MIDIService } from '../../services/midiService';
import { HelperService } from '../../services/helperService';
import { ProfileService } from '../../services/profileService';
import { DropdownOption, DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';
import { ProfileComponent } from '../../components/profile/profile.component';
import { TranslationComponent } from '../../components/translation/translation.component';

@Component({
    selector: 'transformation',
    templateUrl: './transformation.component.html',
	providers: [MIDIService, HelperService]
})

export class TransformationComponent implements OnInit, OnDestroy {

	private subscriptions: Subscription[];
	private inputDevices: MIDIInputDevice[];
	private outputDevices: MIDIOutputDevice[];

	@Input() form: FormGroup;

	constructor(private midiService: MIDIService, private helperService: HelperService) {
		console.log("constructer transformation component");
		console.log(this.form);
	}

	ngOnInit(): void {
		this.subscriptions = new Array<Subscription>();
		this.subscriptions.push(this.midiService.availableInputDevicesChanges
			.subscribe(data => {
				this.inputDevices = data.map(device => this.helperService.maskCast(device, MIDIInputDevice));
			}));
		this.subscriptions.push(this.midiService.availableOutputDevicesChanges
			.subscribe(data => {
				this.outputDevices = data.map(device => this.helperService.maskCast(device, MIDIOutputDevice));
			}));

		
		console.log("onInit transformation component");
		console.log(this.form);
		console.log("inputDevices:");
		console.log(this.inputDevices);
	}

	ngOnDestroy(): void {
		this.subscriptions.forEach(s => s.unsubscribe());
	}
}