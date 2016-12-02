import { Component, ViewChild, Injectable, Input, Output, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import '../rxjs-operators';
import { Subscription } from 'rxjs/Subscription';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption } from '../models/domainModel';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Subject } from 'rxjs/Subject';
import {SlimLoadingBarService} from 'ng2-slim-loading-bar';


@Injectable()
export class ProfileService {

	private profileURL: string = "http://localhost:9000/midi/profile";
	private innerProfile: Profile;
	private form: FormGroup;
	public profileChanges = new Subject<Profile>();
	private subscriptions: { [name: string]: Subscription; };

    constructor(private http: Http,
		private slimLoadingBarService: SlimLoadingBarService,
		private fb: FormBuilder) {
		this.subscriptions['profileChanges'] = (this.profileChanges
            .subscribe(data => {
                this.profile = data;
                this.form = this.getProfileFormGroup(this.profile);
                this.realtimeService.handleRealtimeForForm(this.form);
            }));
    }

	get profile(): Profile {
		return this.innerProfile;
	}

    public getProfileFromServer() {
        this.slimLoadingBarService.start();
		this.http.get(this.profileURL)
            .map(response => <Profile>response.json())
            .subscribe(data => {
				this.profileChanges.next(data);
				this.slimLoadingBarService.complete();
			},
            err => console.log(err));
    }

    public postProfile(profile: Profile) {
        this.slimLoadingBarService.start();
		this.http.post("http://localhost:9000/midi/profile", profile)
			.map(response => <Profile>response.json())
			.subscribe(data => {
				this.profileChanges.next(data);
				this.slimLoadingBarService.complete();
			},
			err => console.log(err));
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
                outputDevice: this.fb.group({
                    deviceID: [transformation.outputDevice.deviceID],
                    driverVersion: [transformation.outputDevice.driverVersion],
                    mid: [transformation.outputDevice.mid],
                    name: [transformation.outputDevice.name],
                    pid: [transformation.outputDevice.pid],
                    support: [transformation.outputDevice.support],
                    label: [transformation.outputDevice.label],
                    value: [transformation.outputDevice.value]
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
                inputMessageMatchTarget: this.getChannelMessageFormGroup(<ChannelMessage>translation.inputMessageMatchTarget),
                outputMessageTemplate: this.getChannelMessageFormGroup(<ChannelMessage>translation.outputMessageTemplate),
                translationFunction: [translation.translationFunction, [<any>Validators.required]]
            }))
        );

        return returnValue;
    }

    private getChannelMessageFormGroup(channelMessage: ChannelMessage): FormGroup {
        return this.fb.group({
            $type: [channelMessage["$type"]],
            command: [channelMessage.command, [<any>Validators.required]],
            data1: [channelMessage.data1, [<any>Validators.required]],
            data2: [channelMessage.data2, [<any>Validators.required]],
            midiChannel: [channelMessage.midiChannel, [<any>Validators.required]],
        });
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
}