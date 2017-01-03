import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck, OnInit, OnDestroy, ChangeDetectionStrategy, trigger, state, style, transition, animate, OnChanges, SimpleChanges, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../../rxjs-operators';
import { EnumValues } from 'enum-values';
import { MIDIService } from '../../services/midiService';
import { HelperService } from '../../services/helperService';
import { RealtimeService } from '../../services/realtimeService';
import { ProfileService } from '../../services/profileService';
import { SignalRService, ChannelEvent } from '../../services/signalRService';
import { DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { DropdownOption } from '../../components/mdl-dropdown/dropdownOption';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';
import { ProfileComponent } from '../../components/profile/profile.component';
import { ChannelMessageComponent } from '../../components/channelMessage/channelMessage.component';

declare var componentHandler;

@Component({
    selector: 'translation',
    templateUrl: './translation.component.html'
})

export class TranslationComponent implements OnInit, OnDestroy {

    private subscriptions: Subscription[];
    private immtReaderSubscription: Subscription;
    private omtReaderSubscription: Subscription;
    private inputMatchFunctions: IDropdownOption[];
    private translationFunctions: IDropdownOption[];

    @Input() form: FormGroup;
    @Input() inputDevice: MIDIInputDevice;
    @Input() index: number;
    @Output() deleteTranslationChange = new EventEmitter();
    
    private readingIMMT: Boolean;
    private readingOMT: Boolean;

    constructor(private midiService: MIDIService,
        private helperService: HelperService,
        private signalRService: SignalRService,
        private cdr: ChangeDetectorRef,
		private realtimeService: RealtimeService,
		private profileService: ProfileService) {
    }

    ngOnInit(): void {
        let component = this;

        this.subscriptions = new Array<Subscription>();
		this.subscriptions.push(this.midiService.availableInputMatchFunctionsSubject
			.subscribe(data => {
				this.inputMatchFunctions = data.map(
					fx => new DropdownOption(InputMatchFunction[fx].toString(), InputMatchFunction[fx].toString())
				);
			}));

        this.subscriptions.push(this.midiService.availableTranslationFunctionsSubject
            .subscribe(data => this.translationFunctions = data.map(fx => new DropdownOption(TranslationFunction[fx].toString(), TranslationFunction[fx].toString()))));

		if (this.midiService.availableInputMatchFunctions != null)
			this.inputMatchFunctions = this.midiService.availableInputMatchFunctions.map(fx => new DropdownOption(InputMatchFunction[fx].toString(), InputMatchFunction[fx].toString()));
		if (this.midiService.availableTranslationFunctions != null)
			this.translationFunctions = this.midiService.availableTranslationFunctions.map(fx => new DropdownOption(TranslationFunction[fx].toString(), TranslationFunction[fx].toString()));
		
        //this.immtReaderSubscription = this.signalRService.sub("tasks")
        //    .subscribe(
        //    (x: ChannelEvent) => {
        //        console.log("wtfffffff");
        //        switch (x.name) {
        //            case "midiChannelEvent":
        //                {
        //                    (<FormControl>component.form.controls["inputMessageMatchTarget"]).setValue(<ChannelEvent>x.data);
        //                    this.cdr.detectChanges();
        //                }
        //        }
        //    },
        //    (error: any) => {
        //        console.log("Attempt to join channel failed!", error);
        //    });
        //this.midiService.startMIDIReader(this.inputDevice.name);
    }

    ngOnDestroy(): void {
        if (this.readingIMMT) {
            this.toggleReadingIMMT();
        }
        this.subscriptions.forEach(s => s.unsubscribe());
    }

	private stopMIDIReader(midiReaderSubscription: Subscription) {
		this.midiService.stopMIDIReader(this.inputDevice.name);
		midiReaderSubscription.unsubscribe();
		this.profileService.saveProfile();
		this.realtimeService.enableRealtime();
	}

	private startMIDIReader(controlName: string): Subscription {
		let component = this;
		this.realtimeService.disableRealtime();
		let subscription = this.signalRService.sub("tasks")
			.subscribe(
			(x: ChannelEvent) => {
				switch (x.name) {
					case "midiChannelEvent":
						{
							(<FormControl>component.form.controls[controlName]).setValue(<ChannelEvent>x.data);
							this.cdr.detectChanges();
						}
				}
			},
			(error: any) => {
				console.log("Attempt to join channel failed!", error);
			});
		this.midiService.startMIDIReader(this.inputDevice.name);
		return subscription;
	}

    private toggleReadingIMMT() {
        this.readingIMMT = !this.readingIMMT;
        if (this.readingIMMT) {
			this.immtReaderSubscription = this.startMIDIReader("inputMessageMatchTarget");
        } else {
            this.stopMIDIReader(this.immtReaderSubscription);
        }
    }

    private toggleReadingOMT() {

        this.readingOMT = !this.readingOMT;
		if (this.readingOMT) {
			this.omtReaderSubscription = this.startMIDIReader("outputMessageTemplate");
        } else {
			this.stopMIDIReader(this.omtReaderSubscription);
        }
    }

    private deleteTranslation() {
        this.deleteTranslationChange.next(this.index);
    }
}