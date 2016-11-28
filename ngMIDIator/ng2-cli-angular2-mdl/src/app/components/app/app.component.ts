//domain model
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption, TranslationMap } from '../../models/domainModel';

import * as $ from 'jquery';

//services
import { MIDIService } from '../../services/midiService';
import { HelperService } from '../../services/helperService';
import { ProfileService } from '../../services/profileService';

//components
import { DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { DropdownOption } from '../../components/mdl-dropdown/dropdownOption';

//ng2
import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck, OnInit, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../../rxjs-operators';

//libs
import { EnumValues } from 'enum-values';

declare var componentHandler;


@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    providers: [MIDIService, HelperService, ProfileService]
})
export class AppComponent implements OnInit, OnDestroy {

    private profile: Profile;
    private form: FormGroup;
    private inputDevices: MIDIInputDevice[];
    private subscriptions: Subscription[];

    constructor(private fb: FormBuilder,
        private midiService: MIDIService,
        private helperService: HelperService,
        private profileService: ProfileService,
        private cdr: ChangeDetectorRef) {
    }

    private upgradeAllElements() {
        componentHandler.upgradeAllRegistered();
    }

    ngOnInit() {
        this.subscriptions = new Array<Subscription>();
        this.subscriptions.push(this.profileService.profileChanges
            .subscribe(data => {
                this.profile = data;
                this.form = this.getProfileFormGroup(this.profile);
                this.subscriptions.push(this.form.valueChanges.debounceTime(400).subscribe(values => this.save(values, true))); //todo: this might get called multiple times since we're adding a subscription inside the continuation of the async call
                //setTimeout(() =>
                //    componentHandler.upgradeAllRegistered());
            }));
        
        this.profileService.getProfile();
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

    ngOnDestroy() {
        this.subscriptions.forEach(s => s.unsubscribe());
    }

    save(model: Profile, isValid: boolean) {
        console.log(model, isValid);
        if (isValid)
            this.profileService.postProfile(model);
    }
}


