//import { Component, ViewChild, Injectable, Input, Output, EventEmitter } from '@angular/core';
//import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
//import { Http } from '@angular/http';
//import { Observable } from 'rxjs/Observable';
//import './rxjs-operators';
//import { EnumValues } from 'enum-values';
//import { ProfileService } from './profileService';
//import { Subscription } from 'rxjs/Subscription';
//import { IDropdownOption, DropdownComponent } from './mdl-dropdown.component';
//import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from './domainModel';
//import { ProfileComponent } from './profile.component';
//import { TranslationComponent } from './translation.component';

//import { Subject } from 'rxjs/Subject';

import '../../midiator-global-imports'

@Injectable()
export class ProfileService {

    private profileUrl: string = "http://localhost:9000/midi/profile";

    constructor(private http: Http) {

    }

    getProfile(name: string, callback: (data: Observable<Profile>) => void) {
        this.http.get(this.profileUrl)
            .map(response => response.json())
            .subscribe(data => {
                callback(<Observable<Profile>>data);
                console.log(data);
            },
            err => console.log(err));
    }
}