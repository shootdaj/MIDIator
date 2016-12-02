import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, IDropdownOption, TranslationMap } from '../../models/domainModel';
import * as $ from 'jquery';
import { MIDIService } from '../../services/midiService';
import { HelperService } from '../../services/helperService';
import { ProfileService } from '../../services/profileService';
import { RealtimeService } from '../../services/realtimeService';
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
    providers: [MIDIService, HelperService, ProfileService]
})
export class AppComponent implements OnInit, OnDestroy {

    private profile: Profile;
    private form: FormGroup;
    private subscriptions: { [name: string]: Subscription; };
	//public realtime: Boolean = true;

	//public set profileRealtime(inValue: Boolean) {
	//	if (inValue) {
	//		this.enableRealtime();
	//	}
	//	else
	//		this.disableRealtime();
	//}

	//public get profileRealtime(): Boolean {
	//	return this.realtime;
	//}

    constructor(private midiService: MIDIService,
        private helperService: HelperService,
        private profileService: ProfileService,
        private realtimeService: RealtimeService) {
    }

	ngOnInit() {
        this.subscriptions = {};
        
        
        this.profileService.getProfileFromServer();
    }

	public getRealtimeTooltip() {
		let returnValue = this.realtimeService.isRealtime() ? "Disable Realtime" : "Enable Realtime";
		return returnValue;
	}

 //   private disableRealtime() {
	//    console.log('disabling realtime');

 //       this.realtime = false;
 //       this.subscriptions['formValueChanges'].unsubscribe();
 //       this.subscriptions['formValueChanges'] = null;
 //   }

 //   private enableRealtime() {
	//	console.log('enabling realtime');

        
 //   }
	
	//private getRealtimeTooltip() {
	//	let returnValue = this.realtime ? "Disable Realtime" : "Enable Realtime";
	//	console.log("message = " + returnValue);
	//	return returnValue;
	//}

    
    ngOnDestroy() {
        (<any>this.subscriptions).children.forEach(s => (<Subscription>s).unsubscribe());
    }

    save(model: Profile, isValid: boolean) {
        console.log(model, isValid);
        if (isValid)
            this.profileService.postProfile(model);
    }

	
}


