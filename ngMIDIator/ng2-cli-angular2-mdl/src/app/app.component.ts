import { Component, ViewChild, Injectable } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl,
	Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import './rxjs-operators';
import { EnumValues } from 'enum-values';

import { MIDIInputDevice, MIDIOutputDevice, Translation, ShortMessage,
	InputMatchFunction, TranslationFunction, MessageType, Profile,
	Transformation, ChannelCommand } from './base';

import { MIDIService } from './midiService'
import { ProfileComponent } from './profile.component'
import { IDropdownOption, DropdownComponent } from './mdl-dropdown.component';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	providers: [MIDIService]
})
export class AppComponent {

	public currentProfile: Profile;

	public get transformations(): Transformation[] {
        return this.currentProfile.transformations;
    }

	constructor(private midiService: MIDIService, private http: Http) {
		midiService.getProfile("" /* put some profile name here later */,
			(data) => {
				this.currentProfile = data;
				console.log(this.currentProfile);
			});

		this.midiService.getAvailableInputDevices(data => this.availableInputDevices = data);
		this.midiService.getAvailableOutputDevices(data => this.availableOutputDevices = data);
		this.midiService.getAvailableChannelCommands(data => this.availableChannelCommands = data);
		this.midiService.getAvailableMIDIChannels(data => this.availableMIDIChannels = data);
	}

	//public availableInputDevices: Observable<Array<IDropdownOption>>;
	//public availableOutputDevices: Observable<Array<IDropdownOption>>;
	//public channelCommands: Observable<Array<IDropdownOption>>;
	//public midiChannels: Observable<Array<IDropdownOption>>;

	//availableInputDevices: Observable<Array<MIDIInputDevice>>;
	//availableOutputDevices: Observable<Array<MIDIOutputDevice>>;
	//availableChannelCommands: Observable<Array<ChannelCommand>>;
	//availableMIDIChannels: Observable<Array<number>>;

	availableInputDevices: MIDIInputDevice[];
	availableOutputDevices: MIDIOutputDevice[];
	availableChannelCommands: ChannelCommand[];
	availableMIDIChannels: number[];
	
	//public onInputDeviceSelected(device) {
	//	this.selectedInputDevice = device;
	//	console.log(device);
	//}

	//public onOutputDeviceSelected(device) {
	//	this.selectedOutputDevice = device;
	//}

	//public getAvailableInputDevices() {
	//	this.http.get('http://localhost:9000/midi/AvailableInputDevices')
	//		.map(response => response.json())
	//		.subscribe(data => this.availableInputDevices =
	//			data,//.map(device => { return new DropdownOption(device.Name, device.Name); })
	//		err => console.log(err));
	//}

	//public getAvailableOutputDevices() {
	//	this.http.get('http://localhost:9000/midi/AvailableOutputDevices')
	//		.map(response => response.json())
	//		.subscribe(data => this.availableOutputDevices =
	//			data,//.map(device => { return new DropdownOption(device.Name, device.Name); }),
	//		err => console.log(err));
	//}

	//public getChannelCommands() {
	//	this.http.get('http://localhost:9000/midi/ChannelCommands')
	//		.map(response => response.json())
	//		.subscribe(data => this.availableChannelCommands =
	//			data,//.map(channelCommand => { return new DropdownOption(channelCommand, channelCommand); }),
	//		err => console.log(err));
	//}

	//public getMIDIChannels() {
	//	this.http.get('http://localhost:9000/midi/MIDIChannels')
	//		.map(response => response.json())
	//		.subscribe(data => this.availableMIDIChannels =
	//			data,//.map(channel => { return new DropdownOption(channel, channel); }),
	//		err => console.log(err));
	//}
}

