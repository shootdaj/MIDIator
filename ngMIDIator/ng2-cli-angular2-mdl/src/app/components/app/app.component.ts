//domain model
import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';

//services
import { MIDIService } from '../../services/midiService';
import { ProfileService } from '../../services/profileService';

//components
import { IDropdownOption, DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { TransformationComponent } from '../../components/transformation/transformation.component';
import { ProfileComponent } from '../../components/profile/profile.component';

//ng2
import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../../rxjs-operators';

//libs
import { EnumValues } from 'enum-values';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	providers: [MIDIService, ProfileService]
})
export class AppComponent {

	public currentProfile: Observable<Profile> = new Observable<Profile>();

	//public get transformations(): Transformation[] {
 //       return this.currentProfile.transformations;
 //   }

	constructor(private midiService: MIDIService, private profileService: ProfileService, private http: Http) {
		profileService.getProfile("" /* put some profile name here later */,
			(data) => {
				this.currentProfile = data;
				console.log(this.currentProfile);
			});
		//this.getProfile();

		//this.midiService.getAvailableInputDevices(data => this.availableInputDevices = data);
		//this.midiService.getAvailableOutputDevices(data => this.availableOutputDevices = data);
		//this.midiService.getAvailableChannelCommands(data => this.availableChannelCommands = data);
		//this.midiService.getAvailableMIDIChannels(data => this.availableMIDIChannels = data);
	}
	
	public getProfile() {
		this.http.get('http://localhost:9000/midi/Profile')
			.map(response => response.json())
			.subscribe(data => this.currentProfile = data,
			err => console.log(err));
    }

	//public availableInputDevices: Observable<Array<IDropdownOption>>;
	//public availableOutputDevices: Observable<Array<IDropdownOption>>;
	//public channelCommands: Observable<Array<IDropdownOption>>;
	//public midiChannels: Observable<Array<IDropdownOption>>;

	//availableInputDevices: Observable<Array<MIDIInputDevice>>;
	//availableOutputDevices: Observable<Array<MIDIOutputDevice>>;
	//availableChannelCommands: Observable<Array<ChannelCommand>>;
	//availableMIDIChannels: Observable<Array<number>>;

	//availableInputDevices: MIDIInputDevice[];
	//availableOutputDevices: MIDIOutputDevice[];
	//availableChannelCommands: ChannelCommand[];
	//availableMIDIChannels: number[];
	
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

