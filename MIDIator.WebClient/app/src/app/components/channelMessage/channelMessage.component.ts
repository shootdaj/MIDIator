import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
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
	selector: 'channel-message',
    templateUrl: './channelMessage.component.html'
})

export class ChannelMessageComponent implements OnInit, OnDestroy {

	private subscriptions: Subscription[];
	private channelCommands: IDropdownOption[];
	private midiChannels: IDropdownOption[];

	@Input() form: FormGroup;

	constructor(private midiService: MIDIService, private helperService: HelperService) {
	}

	ngOnInit(): void {
		this.subscriptions = new Array<Subscription>();
		this.subscriptions.push(this.midiService.availableChannelCommandsSubject
            .subscribe(data => this.channelCommands = data.map(fx =>
                new DropdownOption(ChannelCommand[fx].toString(), ChannelCommand[fx].toString()))));

		this.subscriptions.push(this.midiService.availableMIDIChannelsSubject
            .subscribe(data => this.midiChannels = data.map(fx =>
                new DropdownOption(fx.toString(), fx.toString()))));

		if (this.midiService.availableChannelCommands != null)
			this.channelCommands = this.midiService.availableChannelCommands.map(fx => new DropdownOption(ChannelCommand[fx].toString(), ChannelCommand[fx].toString()));
		if (this.midiService.availableMIDIChannels != null)
			this.midiChannels = this.midiService.availableMIDIChannels
				.map(fx => new DropdownOption(fx.toString(), fx.toString()));
	}

	ngOnDestroy() {
		this.subscriptions.forEach(s => s.unsubscribe());
	}
}