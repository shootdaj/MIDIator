import { Component, ViewChild, Injectable, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../../rxjs-operators';
import { EnumValues } from 'enum-values';
import { MIDIService } from '../../services/midiService';
import { ProfileService } from '../../services/profileService';
import { DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { DropdownOption } from '../../components/mdl-dropdown/dropdownOption';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, TranslationMap } from '../../models/domainModel';
import { TransformationComponent } from '../../components/transformation/transformation.component';
import { TranslationComponent } from '../../components/translation/translation.component';

@Component({
    selector: 'profile',
    templateUrl: './profile.component.html'
})

export class ProfileComponent implements OnInit, OnDestroy {

	@Input() form: FormGroup;

	constructor() {
	}

    //private subscriptions: Subscription[];
    //private currentProfile: Profile;

    //private currentForm: FormGroup;
    //@Output() currentFormChange: EventEmitter<FormGroup> = new EventEmitter<FormGroup>();

    //@Input() set form(form: FormGroup) {
    //    this.currentForm = form;
    //    this.currentFormChange.emit(form);
    //}
    //get form() {
    //    return this.currentForm;
    //}

    //@Input() set profile(inProfile: Profile) {
    //    this.currentProfile = inProfile;
    //    this.profileChange.emit(inProfile);
    //}
    //get profile(): Profile {
    //    return this.currentProfile;
    //}

    //@Output() profileChange: EventEmitter<Profile> = new EventEmitter<Profile>();


    //private form: FormGroup;
    //constructor(fb: FormBuilder) {
    //    this.form = fb.group({
    //        "name": this.currentProfile.name,
    //        "transformations": this.currentProfile.transformations
    //    });
    //}
	ngOnInit(): void {
	}

	ngOnDestroy(): void {}
}