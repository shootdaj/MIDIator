import { Component, ViewChild, Injectable, Input, Output, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import '../rxjs-operators';
import { Subscription } from 'rxjs/Subscription';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption } from '../models/domainModel';
import { Subject } from 'rxjs/Subject';


@Injectable()
export class ProfileService {

    public profileChanges = new Subject<Profile>();

    constructor(private http: Http) {

    }

    getProfile() {
	    this.http.get("http://localhost:9000/midi/profile")
            .map(response => <Profile>response.json())
            .subscribe(data => this.profileChanges.next(data),
            err => console.log(err));
    }
}