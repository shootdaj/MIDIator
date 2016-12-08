import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck, OnInit, OnDestroy, ChangeDetectionStrategy, trigger, state, style, transition, animate, OnChanges, SimpleChanges, AfterViewInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../../rxjs-operators';
import { EnumValues } from 'enum-values';
import { MIDIService } from '../../services/midiService';
import { HelperService } from '../../services/helperService';
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
	private inputMatchFunctions: IDropdownOption[];
	private translationFunctions: IDropdownOption[];

    @Input() form: FormGroup;

	private readingIMMT: Boolean;
	private readingOMT: Boolean;
	private immtClass: string = "";

	constructor(private midiService: MIDIService,
		private helperService: HelperService,
		private signalRService: SignalRService) {
	}

	ngOnInit(): void {
		this.subscriptions = new Array<Subscription>();
		this.subscriptions.push(this.midiService.availableInputMatchFunctionsSubject
            .subscribe(data => this.inputMatchFunctions = data.map(fx => new DropdownOption(InputMatchFunction[fx].toString(), InputMatchFunction[fx].toString()))));

		this.subscriptions.push(this.midiService.availableTranslationFunctionsSubject
            .subscribe(data => this.translationFunctions = data.map(fx => new DropdownOption(TranslationFunction[fx].toString(), TranslationFunction[fx].toString()))));

		this.midiService.getAvailableInputMatchFunctions();
		this.midiService.getAvailableTranslationFunctions();
	}

	ngOnDestroy(): void {
		this.subscriptions.forEach(s => s.unsubscribe());
	}

	private toggleReadingIMMT() {
		this.readingIMMT = !this.readingIMMT;

		if (this.readingIMMT) {
			this.immtReaderSubscription = this.signalRService.sub("tasks")
				.subscribe(
					(x: ChannelEvent) => {
						switch (x.Name) {
						case "midiChannelEvent":
						{
							this.form.controls['inputMessageMatchTarget'].setValue(<ChannelEvent>x.Data);
						}
						}
					},
					(error: any) => {
						console.log("Attempt to join channel failed!", error);
					}
				);
		} else {
			this.immtReaderSubscription.unsubscribe();
		}
	}

	private toggleReadingOMT() {
		this.readingOMT = !this.readingOMT;
	}
}