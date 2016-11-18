import { Component, ViewChild, Injectable, Input, Output, EventEmitter } from '@angular/core';
//import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import '../rxjs-operators';
import { Subscription } from 'rxjs/Subscription';
//import { IDropdownOption, DropdownComponent } from './mdl-dropdown.component';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption } from '../models/domainModel';

import { Subject } from 'rxjs/Subject';

//import '../../midiator-global-imports'

@Injectable()
export class ProfileService {

    private profileUrl: string = "http://localhost:9000/midi/profile";

	public profileChanges = new Subject<Profile>();

    constructor(private http: Http) {

    }

    getProfile() {
        this.http.get(this.profileUrl)
            .map(response => response.json())
            .subscribe(data => this.profileChanges.next(data),
            err => console.log(err));
    }
}