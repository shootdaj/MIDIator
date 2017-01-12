import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../rxjs-operators';
import { EnumValues } from 'enum-values';
import { ProfileService } from '../services/profileService';
import { IDropdownOption, IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, TranslationMap } from '../models/domainModel';
import { DropdownOption } from '../components/mdl-dropdown/dropdownOption';
import {FormService} from "./formService";
import * as $ from 'jquery';

@Injectable()
export class HelperService {
    constructor(private http: Http, private formService: FormService) {

    }

    maskCast(rawObj, constructor) {
        var obj = new constructor();
        for (var i in rawObj)
            obj[i] = rawObj[i];
        return obj;
    }

    deepRemove(obj, name) {
        delete obj[name];
        Object.keys(obj).forEach(key => {
            if (obj[key] instanceof Object)
                this.deepRemove(obj[key], name);
        });
    };

    maskCastProfile(rawProfile): Profile {
        var profile = new Profile();

        this.deepRemove(rawProfile, "label");
        this.deepRemove(rawProfile, "value");

        profile.name = rawProfile.name;
        rawProfile.transformations.forEach(rawXForm => {
            var xform = new Transformation();

            xform.inputDevice = new MIDIInputDevice();
            $.extend(xform.inputDevice, rawXForm.inputDevice);

            xform.outputDevice = new MIDIOutputDevice();
            $.extend(xform.outputDevice, rawXForm.outputDevice);

            profile.transformations.push(xform);
        });

        return profile;
    }

    getDropdownOption(input: any): DropdownOption {
        return new DropdownOption(input.name, input.name);
    }

    dropdownOptionValueSetFunction(inValue: any, options: IDropdownOption[], control: FormGroup): any {
        let value = options.filter(x => x.value === inValue)[0];
        control.setValue(value);
    }

    dropdownOptionValueGetFunction(control: FormGroup): any {
        return control.value.value;
    }

    ifNotNull(expressionToTest, valueToSet): any {
        return expressionToTest != null ? valueToSet : null;
    }

    public initTranslation() {
        let translation = new Translation();
        //(<any>translation).inputMatchFunction = InputMatchFunction[InputMatchFunction.NoteMatch];

        var channelMessage = new ChannelMessage();
        //(<any>channelMessage).command = ChannelCommand[ChannelCommand.ChannelPressure];
        //channelMessage.data1 = 0;
        //channelMessage.data2 = 0;
        //channelMessage.midiChannel = 1;

        translation.inputMessageMatchTarget = channelMessage;
        //(<any>translation).translationFunction = TranslationFunction[TranslationFunction.ChangeNote];
        translation.outputMessageTemplate = channelMessage;

        return translation;
    }

    public generateUUID() {
        var d = new Date().getTime();
        if (window.performance && typeof window.performance.now === "function") {
            d += performance.now(); //use high-precision timer if available
        }
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    }

    public initTransformation(name: string) {
        let transformation = new Transformation();
        transformation.id = this.generateUUID();
        transformation.name = name;
        transformation.inputDevice = null;
        transformation.outputDevice = null;
        let translationMap = new TranslationMap();
        //translationMap.translations.push(this.initTranslation());
        transformation.translationMap = translationMap;
        transformation.linkedOutputVirtualDevice = false;
        transformation.enabled = true;

        return transformation;
    }
}