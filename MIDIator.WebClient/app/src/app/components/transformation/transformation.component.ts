import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck, OnInit, AfterViewInit, OnDestroy, trigger, state, style, transition, animate, ChangeDetectorRef } from '@angular/core';
import { FormsModule, FormArray, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../../rxjs-operators';
import { EnumValues } from 'enum-values';
import { MIDIService } from '../../services/midiService';
import { HelperService } from '../../services/helperService';
import { ProfileService } from '../../services/profileService';
import { FormService } from '../../services/formService';
import { DropdownOption } from '../../components/mdl-dropdown/dropdownOption';
import { DropdownComponent } from '../../components/mdl-dropdown/mdl-dropdown.component';
import { TranslationMap, IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';
import { ProfileComponent } from '../../components/profile/profile.component';
import { TranslationComponent } from '../../components/translation/translation.component';
import { ConnectionState, SignalRService, ChannelEvent } from '../../services/signalRService';

@Component({
    selector: 'transformation',
    templateUrl: './transformation.component.html',
    styles: ['.adjust-right-alignment { margin-right: 16px;}']
})

export class TransformationComponent implements OnInit, OnDestroy {

    private subscriptions: Subscription[];
    private inputDevices: MIDIInputDevice[];
    private outputDevices: MIDIOutputDevice[];
	private blinkTransformation = false;
    private blinkTransformationForward = false;

    @Input() form: FormGroup;
    @Output() deleteTransformationChange = new EventEmitter();

    constructor(private midiService: MIDIService,
        private helperService: HelperService,
        private formService: FormService,
		private signalRService: SignalRService,
		private cdr: ChangeDetectorRef) {
    }

    ngOnInit(): void {
        this.subscriptions = new Array<Subscription>();
        this.subscriptions.push(this.midiService.availableInputDevicesChanges
            .subscribe(data => {
                this.inputDevices = data.map(device => this.helperService.maskCast(device, MIDIInputDevice));
            }));
        this.subscriptions.push(this.midiService.availableOutputDevicesChanges
            .subscribe(data => {
                this.outputDevices = data.map(device => this.helperService.maskCast(device, MIDIOutputDevice));
            }));

        this.midiService.getAvailableInputDevices();
        this.midiService.getAvailableOutputDevices();

	    this.startBroadcastListener();
    }

	private startBroadcastListener() {
		if (this.signalRService.currentState === ConnectionState.Connected) {
			console.log("signalR already connected - subscribing immediately");
			this.subscribeToBroadcast();
		} else {
			console.log("signalR NOT connected - setting subscription to subscribe (HA!)");
			let sub = this.signalRService.connectionState$.subscribe(state => {
				if (state === ConnectionState.Connected) {
					this.subscribeToBroadcast();
					console.log("unsubscribing to the subscription subscribe");
					sub.unsubscribe();
				}
			});
		}
	}

	private subscribeToBroadcast() {
		let component = this;
		this.subscriptions.push(this.signalRService.sub("tasks")
			.subscribe(
			(x: ChannelEvent) => {
				switch (x.name) {
                    case "transformationBroadcastEvent":
                        {
                            let broadcastPayload = x.data;
                            console.log(broadcastPayload);

                            // if the id of the incoming broadcast matches the id of the translation this component represents, then blink it
                            if (broadcastPayload.inputDevice.deviceID === (<MIDIInputDevice>component.form.controls['inputDevice'].value).deviceID) {
                                component.blinkTransformationForward = true;
                                this.cdr.detectChanges();
                                setTimeout(() => {
                                    component.blinkTransformationForward = false;
                                    this.cdr.detectChanges();
                                },
                                    300);

                            }
                        }
                        break;
                    case "translationBroadcastEvent":
                        {
                            let broadcastPayload = x.data;
                            console.log(broadcastPayload);

                            // if the broadcast event's translation is part of this transformation, then blink it
                            if ((<TranslationMap>component.form.controls['translationMap'].value).translations.map(x => x.id).indexOf(broadcastPayload.translation.id) > -1) {
                                component.blinkTransformation = true;
                                this.cdr.detectChanges();
                                setTimeout(() => {
                                    component.blinkTransformation = false;
                                    this.cdr.detectChanges();
                                },
                                    300);

                            }
                        }
                        break;
				}
			},
			(error: any) => {
				console.log("Attempt to join channel failed!", error);
			}));
	}

    ngOnDestroy(): void {
        this.subscriptions.forEach(s => s.unsubscribe());
    }

    private linkOutputDevice() {
        this.form.controls['linkedOutputVirtualDevice'].setValue(true);
    }

    private unlinkOutputDevice() {
        this.form.controls['linkedOutputVirtualDevice'].setValue(false);
    }

    private get linkedOutputDevice() {
        return this.form.controls['linkedOutputVirtualDevice'].value;
    }

    private addNewTranslation() {
        let control = <FormArray>(<FormGroup>this.form.controls['translationMap']).controls['translations'];
        control.push(this.initTranslationFormGroup());
    }

    private initTranslationFormGroup() {
        return this.formService.getTranslationFormGroup(this.helperService.initTranslation());
    }

    private deleteTransformation() {
        this.deleteTransformationChange.next(this.form.value.name);
    }

    private deleteTranslation(index) {
        let control = <FormArray>((<FormArray>this.form.controls['translationMap']).controls['translations']);
        control.removeAt(index);
    }
}