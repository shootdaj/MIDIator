import { Component, Input, Output, DoCheck, EventEmitter } from '@angular/core';
import { Translation, MIDIInputDevice, MIDIOutputDevice,
	ChannelCommand, InputMatchFunction, TranslationFunction, ChannelMessage } from './base';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { IDropdownOption, DropdownOption, DropdownComponent } from './mdl-dropdown.component';
import { MIDIService } from './midiService'

@Component({
	selector: 'translation',
	templateUrl: './translation.component.html',
	providers: [MIDIService]
})

export class TranslationComponent implements DoCheck {

	private subscriptions: Subscription[];
	private availableInputDevices: MIDIInputDevice[];
	private availableOutputDevices: MIDIOutputDevice[];
	private availableInputMatchFunctions: InputMatchFunction[];
	private availableTranslationFunctions: TranslationFunction[];
	
	@Input() translation: Translation;
	@Output() translationChange: EventEmitter<Translation> = new EventEmitter<Translation>();

	constructor(private midiService: MIDIService) {
		this.subscriptions.push(this.midiService.availableInputDevicesSubject
			.subscribe(data => this.availableInputDevices = data));

		this.subscriptions.push(this.midiService.availableOutputDevicesSubject
			.subscribe(data => this.availableOutputDevices = data));

		this.subscriptions.push(this.midiService.availableInputMatchFunctionsSubject
			.subscribe(data => this.availableInputMatchFunctions = data));

		this.subscriptions.push(this.midiService.availableTranslationFunctionsSubject
			.subscribe(data => this.availableTranslationFunctions = data));
	}

	ngOnDestroy() {
		this.subscriptions.forEach(s => s.unsubscribe());
	}

	ngDoCheck(): void {
		this.translationChange.next(this.translation);
	}


	get availableInputMatchFunctionDropdownOptions(): IDropdownOption[] {
		return this.availableInputMatchFunctions.map(
			fx => new DropdownOption((<number>fx).toString(), InputMatchFunction[fx]));
	}

	get availableTranslationFunctionDropdownOptions(): IDropdownOption[] {
		return this.availableTranslationFunctions.map(
			fx => new DropdownOption((<number>fx).toString(), TranslationFunction[fx]));
	}

	get inputMatchFunctionDropdownOption(): IDropdownOption {
		return new DropdownOption((<number>this.translation.inputMatchFunction).toString(), InputMatchFunction[this.translation.inputMatchFunction]);
	}
	set inputMatchFunctionDropdownOption(value: IDropdownOption) {
		this.translation.inputMatchFunction = parseInt(value.value);
	}

	get translationFunctionDropdownOption(): IDropdownOption {
		return new DropdownOption((<number>this.translation.inputMatchFunction).toString(), InputMatchFunction[this.translation.inputMatchFunction]);
	}
	set translationFunctionDropdownOption(value: IDropdownOption) {
		this.translation.translationFunction = parseInt(value.value);
	}
}