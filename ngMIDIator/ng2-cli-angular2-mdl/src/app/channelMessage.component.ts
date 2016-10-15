import { Component, Input, Output, DoCheck, EventEmitter } from '@angular/core';
import { ChannelMessage, MIDIInputDevice, MIDIOutputDevice, ChannelCommand } from './base';
import { Observable} from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { IDropdownOption, DropdownComponent, DropdownOption } from './mdl-dropdown.component';
import { InputMatchFunction, TranslationFunction } from './base';
import {MIDIService} from './midiService'

@Component({
	selector: 'channel-message',
	templateUrl: './channelMessage.component.html',
	providers: [MIDIService]
})

export class ChannelMessageComponent /* implements DoCheck {*/ {

	private subscriptions: Subscription[];
	private availableChannelCommands: ChannelCommand[];
	private availableMIDIChannels: number[];

	@Input() channelMessage: ChannelMessage;
	@Output() channelMessageChange: EventEmitter<ChannelMessage> = new EventEmitter<ChannelMessage>();

	constructor(private midiService: MIDIService) {
		this.subscriptions.push(this.midiService.availableChannelCommandsSubject
			.subscribe(data => this.availableChannelCommands = data));

		this.subscriptions.push(this.midiService.availableMIDIChannelsSubject
			.subscribe(data => this.availableMIDIChannels = data));
	}

	ngDoCheck(): void {
		//this.data1Change.next(this.data1);
		//this.data2Change.next(this.data2);
		//this.channelCommandChange.next(this.channelCommand);
		//this.midiChannelChange.next(this.midiChannel);
		this.channelMessageChange.next(this.channelMessage);
	}

	ngOnDestroy() {
		this.subscriptions.forEach(s => s.unsubscribe());
	}

	get availableChannelCommandDropdownOptions(): IDropdownOption[] {
		return this.availableChannelCommands.map(
			fx => new DropdownOption((<number>fx).toString(), ChannelCommand[fx]));
	}

	get availableMIDIChannelDropdownOptions(): IDropdownOption[] {
		return this.availableMIDIChannels.map(
			fx => new DropdownOption(fx.toString(), fx.toString()));
	}

	get channelCommandDropdownOption(): IDropdownOption {
		return new DropdownOption((<number>this.channelMessage.channelCommand).toString(), ChannelCommand[this.channelMessage.channelCommand]);
	}
	set channelCommandDropdownOption(value: IDropdownOption) {
		this.channelMessage.channelCommand = parseInt(value.value);
	}

	get midiChannelDropdownOption(): IDropdownOption {
		return new DropdownOption(this.channelMessage.midiChannel.toString(), this.channelMessage.midiChannel.toString());
	}
	set midiChannelDropdownOption(value: IDropdownOption) {
		this.channelMessage.midiChannel = parseInt(value.value);
	}

	//@Input() data1: number;
	//@Output() data1Change: EventEmitter<any> = new EventEmitter();

	//@Input() data2: number;
	//@Output() data2Change: EventEmitter<any> = new EventEmitter();

	//@Input() channelCommand: ChannelCommand;
	//@Output() channelCommandChange: EventEmitter<any> = new EventEmitter();

	//@Input() midiChannel: number;
	//@Output() midiChannelChange: EventEmitter<any> = new EventEmitter();
}