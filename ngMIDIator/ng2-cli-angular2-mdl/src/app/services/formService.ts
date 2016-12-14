import { Component, ViewChild, Injectable, Input, Output, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import '../rxjs-operators';
import { Subscription } from 'rxjs/Subscription';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, TranslationMap, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption } from '../models/domainModel';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Subject } from 'rxjs/Subject';
import {SlimLoadingBarService} from 'ng2-slim-loading-bar';


@Injectable()
export class FormService {

	private form: FormGroup;
	public formChanges = new Subject<FormGroup>();

	public getForm(): FormGroup {
		return this.form;
	}

    constructor(private fb: FormBuilder) {

    }

	public setForm(profile: Profile) {
		this.form = this.getProfileFormGroup(profile);
		this.formChanges.next(this.form);
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
                translationMap: this.getTranslationMapFormGroup(transformation.translationMap),
				linkedOutputVirtualDevice: [transformation.linkedOutputVirtualDevice, [<any>Validators.required]]
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
            returnValue.push(this.getTranslationFormGroup(translation))
        );

        return returnValue;
    }

	public getTranslationFormGroup(translation: Translation) {
		return this.fb.group({
			inputMatchFunction: [translation.inputMatchFunction, [<any>Validators.required]],
			inputMessageMatchTarget: this.getChannelMessageFormGroup(<ChannelMessage>translation.inputMessageMatchTarget),
			outputMessageTemplate: this.getChannelMessageFormGroup(<ChannelMessage>translation.outputMessageTemplate),
			translationFunction: [translation.translationFunction, [<any>Validators.required]]
		});
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