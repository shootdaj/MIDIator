import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption, TranslationMap } from '../../models/domainModel';
import * as $ from 'jquery';
import { MIDIService } from '../../services/midiService';
import { HelperService } from '../../services/helperService';
import { ProfileService } from '../../services/profileService';
import { RealtimeService } from '../../services/realtimeService';
import { FormService } from '../../services/formService';
import { DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { DropdownOption } from '../../components/mdl-dropdown/dropdownOption';
import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck, OnInit, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../../rxjs-operators';
import { EnumValues } from 'enum-values';

declare var componentHandler;


@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    providers: [MIDIService, HelperService, ProfileService, FormService]
})
export class AppComponent implements OnInit, OnDestroy {
    
    constructor(private midiService: MIDIService,
        private helperService: HelperService,
        private profileService: ProfileService,
        private formService: FormService) {
    }

	private get form(): FormGroup {
		return this.formService.getForm();
	}

	ngOnInit() {
        this.profileService.loadProfile();
    }
	
	
	ngOnDestroy() {
    }

    saveProfile() {
	    this.profileService.saveProfile();//this.form.value, this.form.valid);
    }
}