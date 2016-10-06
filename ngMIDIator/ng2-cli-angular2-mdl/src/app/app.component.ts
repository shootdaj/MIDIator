import { Component, ViewChild } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
	FormGroup,
	FormControl,
	Validators,
	FormBuilder
} from '@angular/forms';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import './rxjs-operators';
import { DropdownOption, DropdownComponent } from './mdl-dropdown.component';
import { ITranslation } from './base';
import { ITranslationMap } from './base';
import { MIDIInputDevice } from './base';
import { MIDIOutputDevice } from './base';
import { Translation } from './base';
import { ShortMessage } from './base';
import { InputMatchFunction } from './base';
import { TranslationFunction } from './base';
import { MessageType } from './base';
import { Profile } from './base';
import { Transformation } from './base';
import { MIDIService } from './midiService'

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css'],
	providers: [MIDIService]
})
export class AppComponent {

	public currentProfile : Profile;

	public get transformations(): Transformation[] {
        return this.currentProfile.transformations;
    }

	constructor(private _midiService: MIDIService) {
		_midiService.getProfile("", (data) => {
			this.currentProfile = data;
			console.log(this.currentProfile);
		});

		//this.getAvailableInputDevices();
		//this.getAvailableOutputDevices();
		//this.getChannelCommands();
		//this.getMIDIChannels();
		//this.selectedInputDevice = null;
	}

	public availableInputDevices: Observable<Array<DropdownOption>>;
	public availableOutputDevices: Observable<Array<DropdownOption>>;
	public channelCommands: Observable<Array<DropdownOption>>;
	public midiChannels: Observable<Array<DropdownOption>>;

	public selectedInputDevice: DropdownOption;
	public selectedOutputDevice: DropdownOption;

	public onInputDeviceSelected(device) {
		this.selectedInputDevice = device;
		console.log(device);
	}

	public onOutputDeviceSelected(device) {
		this.selectedOutputDevice = device;
	}

	public getAvailableInputDevices() {
		//this.http.get('http://localhost:9000/midi/AvailableInputDevices')
		//	.map(response => response.json())
		//	.subscribe(data => this.availableInputDevices =
		//		data.map(device => { return new DropdownOption(device.Name, device.Name); }),
		//	err => console.log(err));
	}

	public getAvailableOutputDevices() {
		//this.http.get('http://localhost:9000/midi/AvailableOutputDevices')
		//	.map(response => response.json())
		//	.subscribe(data => this.availableOutputDevices =
		//		data.map(device => { return new DropdownOption(device.Name, device.Name); }),
		//	err => console.log(err));
	}

	public getChannelCommands() {
		//this.http.get('http://localhost:9000/midi/ChannelCommands')
		//	.map(response => response.json())
		//	.subscribe(data => this.channelCommands =
		//		data.map(channelCommand => { return new DropdownOption(channelCommand, channelCommand); }),
		//	err => console.log(err));
	}

	public getMIDIChannels() {
		//this.http.get('http://localhost:9000/midi/MIDIChannels')
		//	.map(response => response.json())
		//	.subscribe(data => this.midiChannels =
		//		data.map(channel => { return new DropdownOption(channel, channel); }),
		//	err => console.log(err));
	}
}

