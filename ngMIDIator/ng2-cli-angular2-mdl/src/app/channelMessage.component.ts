import { Component, Input, Output, DoCheck, EventEmitter } from '@angular/core';
import { ChannelMessage, MIDIInputDevice, MIDIOutputDevice, ChannelCommand } from './base';
import { Observable } from 'rxjs/Observable';
import { IDropdownOption, DropdownComponent } from './mdl-dropdown.component';
import { InputMatchFunction, TranslationFunction } from './base';
//import {MIDIService} from './midiService'

@Component({
	selector: 'channel-message',
	templateUrl: './channelMessage.component.html'
})

export class ChannelMessageComponent implements DoCheck {

	@Input() channelMessage: ChannelMessage;
	@Input() availableChannelCommands: IDropdownOption[];
	@Input() availableMIDIChannels: IDropdownOption[];
	@Input() availableInputMatchFunctions: IDropdownOption[];
	@Input() availableTranslationFunctions: IDropdownOption[];

	constructor() {
		//this.availableChannelCommands = Object.keys(ChannelCommand);
		//this.availableMIDIChannels = midiService.getAvailableMIDIChannels();
		//this.availableInputMatchFunctions = midiService.getAvailableInputMatchFunctions();
		//this.availableTranslationFunctions = midiService.getAvailableTranslationFunctions();
	}

	@Input() data1: number;
	@Output() data1Change: EventEmitter<any> = new EventEmitter();

	@Input() data2: number;
	@Output() data2Change: EventEmitter<any> = new EventEmitter();
	
	@Input() channelCommand: ChannelCommand;
	@Output() commandChange: EventEmitter<any> = new EventEmitter();

	@Input() midiChannel: number;
	@Output() midiChannelChange: EventEmitter<any> = new EventEmitter();
		
	ngDoCheck(): void {
		this.data1Change.next(this.data1);
		this.data2Change.next(this.data2);
		this.commandChange.next(this.channelCommand);
		this.midiChannelChange.next(this.midiChannel);
	}
}