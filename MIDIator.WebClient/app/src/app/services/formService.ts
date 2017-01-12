﻿import { Component, ViewChild, Injectable, Input, Output, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import '../rxjs-operators';
import { Subscription } from 'rxjs/Subscription';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, TranslationMap, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption } from '../models/domainModel';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Subject } from 'rxjs/Subject';
import { SlimLoadingBarService } from 'ng2-slim-loading-bar';
import { CustomValidators } from 'ng2-validation'

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
            transformations: this.fb.array(this.getTransformationsFormGroups(profile.transformations)),
            collapsed: [profile.collapsed, [/*<any>Validators.required*/]]
        });
    }

    public getTransformationsFormGroups(transformations: Transformation[]): FormGroup[] {

        var returnValue = Array<FormGroup>();

        transformations.forEach(transformation =>
            returnValue.push(this.fb.group({
                id: [transformation.id, [<any>Validators.required]],
                name: [transformation.name, [<any>Validators.required]],
                inputDevice: this.fb.group({
                    deviceID: [transformation.inputDevice != null ? transformation.inputDevice.deviceID : null],
                    driverVersion: [transformation.inputDevice != null ? transformation.inputDevice.driverVersion : null],
                    mid: [transformation.inputDevice != null ? transformation.inputDevice.mid : null],
                    name: [transformation.inputDevice != null ? transformation.inputDevice.name : null, [<any>Validators.required]],
                    pid: [transformation.inputDevice != null ? transformation.inputDevice.pid : null],
                    support: [transformation.inputDevice != null ? transformation.inputDevice.support : null],
                    label: [transformation.inputDevice != null ? transformation.inputDevice.label : null],
                    value: [transformation.inputDevice != null ? transformation.inputDevice.value : null]
                }),
                outputDevice: this.fb.group({
                    deviceID: [transformation.outputDevice != null ? transformation.outputDevice.deviceID : null],
                    driverVersion: [transformation.outputDevice != null ? transformation.outputDevice.driverVersion : null],
                    mid: [transformation.outputDevice != null ? transformation.outputDevice.mid : null],
                    name: [transformation.outputDevice != null ? transformation.outputDevice.name : null, [<any>Validators.required]],
                    pid: [transformation.outputDevice != null ? transformation.outputDevice.pid : null],
                    support: [transformation.outputDevice != null ? transformation.outputDevice.support : null],
                    label: [transformation.outputDevice != null ? transformation.outputDevice.label : null],
                    value: [transformation.outputDevice != null ? transformation.outputDevice.value : null]
                }),
                translationMap: this.getTranslationMapFormGroup(transformation.translationMap),
                linkedOutputVirtualDevice: [transformation.linkedOutputVirtualDevice, [<any>Validators.required]],
                enabled: [transformation.enabled, [<any>Validators.required]],
                collapsed: [transformation.collapsed, [/*<any>Validators.required*/]],
                translationsCollapsed: [transformation.translationsCollapsed, [/*<any>Validators.required*/]]
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
            translationFunction: [translation.translationFunction, [<any>Validators.required]],
            enabled: [translation.enabled, [<any>Validators.required]],
            collapsed: [translation.collapsed, [/*<any>Validators.required*/]]
        });
    }

    private getChannelMessageFormGroup(channelMessage: ChannelMessage): FormGroup {
        return this.fb.group({
            $type: [channelMessage["$type"]],
            command: [channelMessage.command, [<any>Validators.required]],
            data1: [channelMessage.data1, Validators.compose([Validators.required, CustomValidators.number, CustomValidators.min(0), CustomValidators.max(127)])],
            data2: [channelMessage.data2, Validators.compose([Validators.required, CustomValidators.number, CustomValidators.min(0), CustomValidators.max(127)])],
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