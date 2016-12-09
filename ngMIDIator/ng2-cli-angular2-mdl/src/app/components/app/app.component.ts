import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption, TranslationMap } from '../../models/domainModel';
import * as $ from 'jquery';
import { MIDIService } from '../../services/midiService';
import { HelperService } from '../../services/helperService';
import { ProfileService } from '../../services/profileService';
import { RealtimeService } from '../../services/realtimeService';
import { FormService } from '../../services/formService';
import { SignalRService, ChannelEvent, ConnectionState } from '../../services/signalRService';
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

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit{

    signalRConnState: Observable<string>;

    constructor(private realtimeService: RealtimeService,
		private profileService: ProfileService,
        private formService: FormService,
        private signalRService: SignalRService) {

        // Let's wire up to the signalr observables
        //
        this.signalRConnState = this.signalRService.connectionState$
            .map((state: ConnectionState) => { return ConnectionState[state]; });

        this.signalRService.error$.subscribe(
            (error: any) => { console.warn(error); },
            (error: any) => { console.error("errors$ error", error); }
        );

        // Wire up a handler for the starting$ observable to log the
        //  success/fail result
        //
        this.signalRService.starting$.subscribe(
            () => { console.log("signalr service has been started"); },
            () => { console.warn("signalr service failed to start!"); }
        );


	    this.realtimeService.attachFormChanges();
        this.profileService.loadProfile();

    }

    ngOnInit() {
        console.log("Starting the channel service");
        this.signalRService.start();
    }

	private get form(): FormGroup {
		return this.formService.getForm();
	}

    saveProfile() {
		this.profileService.saveProfile(true);
    }
}