//domain model
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption, TranslationMap } from '../../models/domainModel';

//services
import { MIDIService } from '../../services/midiService';
import { HelperService } from '../../services/helperService';
import { ProfileService } from '../../services/profileService';

//components
import { DropdownOption, DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';

//ng2
import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck, OnInit, OnDestroy } from '@angular/core';
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
	providers: [MIDIService, HelperService, ProfileService]
})
export class AppComponent implements OnInit, OnDestroy {

	private profile: Profile;
	private form: FormGroup;// = this.fb.group({});
	private inputDevices: MIDIInputDevice[];
	private subscriptions: Subscription[];

	constructor(private fb: FormBuilder,
		private midiService: MIDIService,
		private helperService: HelperService,
		private profileService: ProfileService) {
	}

	ngOnInit() {
		this.subscriptions = new Array<Subscription>();
		this.subscriptions.push(this.profileService.profileChanges
			.subscribe(data => {

				//Object.assign()

				//this.profile = //Object.assign(new Profile(), data);
					//this.helperService.deepMap(data, (a, b) => { return a; }, Profile);
					//this.helperService.maskCast(data, Profile);
					//JSON.parse(data);
					//this.helperService.createInstanceFromJson(Profile, data);
					//var t = window["Profile"];
					//<Profile>this.helperService.maskCopy(data, Profile);
					//data.map(device => this.helperService.deepMap(device, (val, key) => val, Profile));

				//this.profile.__proto__ = 
				var profile = JSON.parse(JSON.stringify(data));
				profile.__proto__ = Profile.prototype;

				this.profile = profile;
				var device = JSON.parse(JSON.stringify(this.profile.transformations[0].inputDevice));

				//MIDIInputDevice.prototype.label = f

				device.__proto__ = MIDIInputDevice.prototype.constructor;
				//this.profile.transformations[0].inputDevice.constructor.prototype = MIDIInputDevice.prototype;

				this.form = this.getProfileFormGroup(profile);
				this.subscriptions.push(this.form.valueChanges.subscribe(values => this.save(values, true))); //todo: this might get called multiple times since we're adding a subscription inside the continuation of the async call
			}));

		//this.subscriptions.push(this.midiService.availableInputDevicesChanges
		//	.subscribe(data => {
		//		this.inputDevices = data.map(device => this.helperService.maskCast(device, MIDIInputDevice));
		//	}));

		//this.midiService.getAvailableInputDevices();
		this.profileService.getProfile();
	}

	private getProfileFormGroup(profile: Profile): FormGroup {
		return this.fb.group({
			name: [profile.name, [<any>Validators.required]],
			transformations: this.fb.array(this.getTransformationsFormGroups(profile.transformations))
		});
	}

	private  getTransformationsFormGroups(transformations: Transformation[]): FormGroup[] {

		var returnValue = Array<FormGroup>();

		transformations.forEach(transformation =>
			returnValue.push(this.fb.group({
				name: [transformation.name, [<any>Validators.required]],
				//inputDevice: [transformation.inputDevice, [<any>Validators.required]],
				inputDevice: this.fb.group({
					deviceID: [transformation.inputDevice.deviceID],
					driverVersion: [transformation.inputDevice.driverVersion],
					mid: [transformation.inputDevice.mid],
					name: [transformation.inputDevice.name],
					pid: [transformation.inputDevice.pid],
					support: [transformation.inputDevice.support],
					label: [transformation.inputDevice.label],
					value: [transformation.inputDevice.value]
				}),
				//outputDevice: [this.getOutputDeviceFormGroup(transformation.outputDevice)],
				outputDevice: this.fb.group({
					deviceID: [transformation.outputDevice.deviceID],
					driverVersion: [transformation.outputDevice.driverVersion],
					mid: [transformation.outputDevice.mid],
					name: [transformation.outputDevice.name],
					pid: [transformation.outputDevice.pid],
					support: [transformation.outputDevice.support],
					label: [transformation.inputDevice.label],
					value: [transformation.inputDevice.value]
				}),
				translationMap: this.getTranslationMapFormGroup(transformation.translationMap)
			}))
		);

		return returnValue;
	}

	private getTranslationMapFormGroup(translationMap: TranslationMap): FormGroup {
		return this.fb.group({
			translations: this.fb.array(this.getTranslationsFormGroups(translationMap.translations))
		});
	}

	private getTranslationsFormGroups(translations: Translation[]): FormGroup[] {
		var returnValue = Array<FormGroup>();

		translations.forEach(translation =>
			returnValue.push(this.fb.group({
				inputMatchFunction: [translation.inputMatchFunction, [<any>Validators.required]],
				inputMessageMatchTarget: [translation.inputMessageMatchTarget, [<any>Validators.required]],
				outputMessageTemplate: [translation.outputMessageTemplate, [<any>Validators.required]],
				translationFunction: [translation.translationFunction, [<any>Validators.required]]
			}))
		);

		return returnValue;
	}

    private getInputDeviceFormGroup(inputDevice: IMIDIInputDevice): FormGroup {
	    return this.fb.group({
		    deviceID: [inputDevice.deviceID],
		    driverVersion: [inputDevice.driverVersion],
		    mid: [inputDevice.mid],
		    name: [inputDevice.name],
		    pid: [inputDevice.pid],
		    support: [inputDevice.support]
	    });
    }

	private getOutputDeviceFormGroup(outputDevice: IMIDIOutputDevice): FormGroup {
		return this.fb.group({
			deviceID: [outputDevice.deviceID],
			driverVersion: [outputDevice.driverVersion],
			mid: [outputDevice.mid],
			name: [outputDevice.name],
			pid: [outputDevice.pid],
			support: [outputDevice.support]
		});
    }
	
	ngOnDestroy() {
		this.subscriptions.forEach(s => s.unsubscribe());
	}

	save(model: Profile, isValid: boolean) {
		console.log(model, isValid);
		this.refresh();
	}

	refresh() {
		this.midiService.getAvailableInputDevices();
	}
}


