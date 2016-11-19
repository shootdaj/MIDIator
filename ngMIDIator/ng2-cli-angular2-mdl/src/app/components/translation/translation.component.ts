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
import { DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { DropdownOption } from '../../components/mdl-dropdown/dropdownOption';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';
import { ProfileComponent } from '../../components/profile/profile.component';

@Component({
	selector: 'translation',
	templateUrl: './translation.component.html',
	providers: [MIDIService]
})

export class TranslationComponent implements OnInit, OnDestroy {

	private subscriptions: Subscription[];
	private inputMatchFunctions: InputMatchFunction[];
	private translationFunctions: TranslationFunction[];

	@Input() form: FormGroup;

	constructor(private midiService: MIDIService) {
	}

	ngOnInit(): void {
		this.subscriptions = new Array<Subscription>();
		this.subscriptions.push(this.midiService.availableInputMatchFunctionsSubject
			.subscribe(data => this.inputMatchFunctions = data));

		this.subscriptions.push(this.midiService.availableTranslationFunctionsSubject
            .subscribe(data => this.translationFunctions = data));
	}

	get inputMatchFunctionsDropdownOptions(): IDropdownOption[] {
		if (this.inputMatchFunctions != null && this.inputMatchFunctions.length > 0) {
			return this.inputMatchFunctions.map(
				fx => new DropdownOption((<number>fx).toString(), InputMatchFunction[fx]));
		} else {
			return null;
		}
	}

	get translationFunctionsDropdownOptions(): IDropdownOption[] {
		if (this.translationFunctions != null && this.translationFunctions.length > 0) {
			return this.translationFunctions.map(
				fx => new DropdownOption((<number>fx).toString(), TranslationFunction[fx]));
		} else {
			return null;
		}
	}

	ngOnDestroy(): void {
		this.subscriptions.forEach(s => s.unsubscribe());
	}
}