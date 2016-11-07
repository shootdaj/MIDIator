//domain model
import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption } from '../../models/domainModel';

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
		console.log("constructer app component");
		console.log(this.form);
	}

	ngOnInit() {
		this.subscriptions = new Array<Subscription>();
		this.subscriptions.push(this.profileService.profileChanges
			.subscribe(data => {
				this.profile = this.helperService.maskCast(data, Profile);
				var profile = this.profile;
				this.form = this.getProfileFormGroup(profile);
				this.subscriptions.push(this.form.valueChanges.subscribe(values => this.save(values, true))); //todo: this might get called multiple times since we're adding a subscription inside the continuation of the async call
			}));

		//this.subscriptions.push(this.midiService.availableInputDevicesChanges
		//	.subscribe(data => {
		//		this.inputDevices = data.map(device => this.helperService.maskCast(device, MIDIInputDevice));
		//	}));

		this.midiService.getAvailableInputDevices();
		this.profileService.getProfile();

		console.log("onInit app component");
		console.log(this.form);
	}

	private getProfileFormGroup(profile: Profile): FormGroup {
		return this.fb.group({
			name: [profile.name, [<any>Validators.required]],
			transformations: this.fb.array(this.getTransformationsFormGroups(profile.transformations))
		});
	}

	private getTransformationsFormGroups(transformations: Transformation[]): FormGroup[] {

		var returnValue = Array<FormGroup>();

		transformations.forEach(transformation =>
			returnValue.push(this.fb.group({
				name: [transformation.name, [<any>Validators.required]],
				inputDevice: [transformation.inputDevice, [<any>Validators.required]],
				outputDevice: [transformation.outputDevice, [<any>Validators.required]],
				translationMap: [this.getTranslationMapFormGroup(transformation.translationMap)]
			}))
		);

		return returnValue;
	}

	private getTranslationMapFormGroup(translationMap: ITranslationMap): FormGroup {
		return this.fb.group({
			translations: this.fb.array([this.getTranslationsFormGroups(translationMap.translations)])
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

	ngOnDestroy() {
		this.subscriptions.forEach(s => s.unsubscribe());
	}

	save(model: any, isValid: boolean) {
		console.log(model, isValid);
		this.refresh();
	}

	refresh() {
		this.midiService.getAvailableInputDevices();
	}
}

