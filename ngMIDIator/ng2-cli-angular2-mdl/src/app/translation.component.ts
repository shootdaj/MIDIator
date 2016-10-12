import { Component, Input, Output, DoCheck, EventEmitter } from '@angular/core';
import { Translation, MIDIInputDevice, MIDIOutputDevice,
	ChannelCommand, InputMatchFunction, TranslationFunction} from './base';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { IDropdownOption, DropdownOption, DropdownComponent } from './mdl-dropdown.component';
import { MIDIService } from './midiService'

@Component({
	selector: 'translation',
	templateUrl: './translation.component.html',
	providers: [ MIDIService ]
})

export class TranslationComponent implements DoCheck{

	availableInputDevices: MIDIInputDevice[];
	availableInputDevicesSubscription: Subscription;

	@Input() translation: Translation;
	//@Input() availableInputDevices: MIDIInputDevice[];
	@Input() availableOutputDevices: MIDIOutputDevice[];
	@Input() availableChannelCommands: ChannelCommand[];
	@Input() availableMIDIChannels: number[];
	@Input() availableInputMatchFunctions: InputMatchFunction[];
	@Input() availableTranslationFunctions: TranslationFunction[];

	constructor(private midiService: MIDIService) {
		this.availableInputDevicesSubscription = this.midiService.availableInputDevicesSubject
			.subscribe(data => this.availableInputDevices = data);
	}

	ngOnDestroy() {
		this.availableInputDevicesSubscription.unsubscribe();
	}

	@Input() inputMatchFunction: DropdownOption;
	@Output() inputMatchFunctionChange: EventEmitter<InputMatchFunction> = new EventEmitter<InputMatchFunction>();

	@Input() translationFunction: DropdownOption;
	@Output() translationFunctionChange: EventEmitter<TranslationFunction> = new EventEmitter<TranslationFunction>();

	@Input() data1: number;
	@Output() data1Change: EventEmitter<any> = new EventEmitter();

	ngDoCheck(): void {
		this.inputMatchFunctionChange.next(InputMatchFunction[this.inputMatchFunction.value]);
		this.translationFunctionChange.next(TranslationFunction[this.translationFunction.value]);
	}

	get availableInputMatchFunctionDropdownOptions(): IDropdownOption[] {
		return this.availableInputMatchFunctions.map(
			fx => new DropdownOption((<number>fx).toString(), InputMatchFunction[fx]));
	}

	get availableTranslationFunctionDropdownOptions(): IDropdownOption[] {
		return this.availableTranslationFunctions.map(
			fx => new DropdownOption((<number>fx).toString(), TranslationFunction[fx]));
	}

	get availableChannelCommandDropdownOptions(): IDropdownOption[] {
		return this.availableChannelCommands.map(
			fx => new DropdownOption((<number>fx).toString(), ChannelCommand[fx]));
	}

	get availableMIDIChannelDropdownOptions(): IDropdownOption[] {
		return this.availableMIDIChannels.map(
			fx => new DropdownOption(fx.toString(), fx.toString()));
	}

	
}