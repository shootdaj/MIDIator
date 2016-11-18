import { Component, ViewChild, Injectable, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import '../rxjs-operators';
import { EnumValues } from 'enum-values';
import { ProfileService } from '../services/profileService';
import { DropdownOption, DropdownComponent } from '../components/mdl-dropdown/mdl-dropdown.component';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../models/domainModel';
import * as $ from 'jquery';

@Injectable()
export class HelperService {
	constructor(private http: Http) {

	}



	maskCast(rawObj, constructor) {
		var obj = new constructor();
		for (var i in rawObj)
			obj[i] = rawObj[i];
		return obj;
	}

	//createInstanceFromJson<T>(objType: { new (): T; }, json: any) {
	//	const newObj = new objType();
	//	const relationships = objType["relationships"] || {};

	//	for (const prop in json) {
	//		if (json.hasOwnProperty(prop)) {
	//			//if (newObj[prop] == null) {
	//			if (relationships[prop] == null) {
	//				newObj[prop] = json[prop];
	//			}
	//			else {
	//				newObj[prop] = this.createInstanceFromJson(relationships[prop], json[prop]);
	//			}
	//			//}
	//			//			else {
	//			//					console.warn(`Property ${prop} not set because it already existed on the object.`);
	//			//		}
	//		}
	//	}

	//	return newObj;
	//}

	deepRemove(obj, name) {
		//if (obj[name]) {
			delete obj[name];
		//}

		Object.keys(obj).forEach(key => {
			if (obj[key] instanceof Object)
				this.deepRemove(obj[key], name);
		});

		//for (var property in obj) {
		//	if (obj[property] instanceof Object)
		//		this.deepRemove(property, name);
		//}
	};


	maskCastProfile(rawProfile): Profile {
		var profile = new Profile();

		this.deepRemove(rawProfile, "label");
		this.deepRemove(rawProfile, "value");

		profile.name = rawProfile.name;
		rawProfile.transformations.forEach(rawXForm => {
			var xform = new Transformation();
			xform.inputDevice = new MIDIInputDevice();

			$.extend(xform.inputDevice, rawXForm.inputDevice);
			xform.outputDevice = new MIDIOutputDevice();
			
			profile.transformations.push(xform);
		});

		

		//profile = this.deepMap(rawProfile, (a, b) => { return a; }, Profile);

		return profile;
	}

	//maskCopy(rawObj, constructor): any {
	//	var obj = new constructor();//Object.create(objectTypeTemplate.prototype);

	//	for (var propertyName in rawObj) {
	//		if(Array.isArray(obj[propertyName])) {
	//			obj[propertyName] = new Array();
	//			for (var arrayItem in rawObj[propertyName]) {
	//				obj[propertyName].push(this.maskCopy(rawObj[propertyName]))
	//			}
	//		} else if (obj[propertyName] instanceof Object) {
	//			obj[propertyName] = this.maskCopy(rawObj[propertyName], obj[propertyName].getTypeMetadata().constructor);
	//		}
	//		else {
	//			obj[propertyName] = rawObj[propertyName];
	//		}
	//	}

	//	return obj;
	//}

	//Object.keys(rawObj).forEach(key => {
	//	if (typeof o)
	//});


	//		console.log(obj);



	//		//console.log(Object.keys(obj));
	////		for (var i in obj) {
	////			//var property = Object.create(i.prototype)

	////			console.log(i);



	////			//	var type = typeof i;
	////			//var property = type();
	////			//var property = Object.create(i);
	////			//property = rawObj[]
	////			//property = rawObj[i];

	//////			obj[property] = 
	////		}

	//		return obj;
	//	}


	//deepMap(obj, f, ctx) {
	//	var self = this;
	//	if (Array.isArray(obj)) {
	//		return obj.map(function (val, key) {
	//			return (typeof val === 'object') ? self.deepMap(val, f, ctx) : f.call(ctx, val, key);
	//		});
	//	} else if (typeof obj === 'object') {
	//		var res = {};
	//		for (var key in obj) {
	//			var val = obj[key];
	//			if (typeof val === 'object') {
	//				res[key] = self.deepMap(val, f, ctx);
	//			} else {
	//				res[key] = f.call(ctx, val, key);
	//			}
	//		}
	//		return res;
	//	} else {
	//		return obj;
	//	}
	//}

	mapObject(obj, fn) {
		return Object.keys(obj)
			.reduce(
			(res, key) => {
				res[key] = fn(obj[key]);
				return res;
			},
			{}
			);
	}

	//deepMap(obj, fn) {
	//	const deepMapper = val => typeof val === 'object' ? this.deepMap(val, fn) : fn(val);
	//	if (Array.isArray(obj)) {
	//		return obj.map(deepMapper);
	//	}
	//	if (typeof obj === 'object') {
	//		return this.mapObject(obj, deepMapper);
	//	}
	//	return obj;
	//}

	clone(obj) {
		if (obj == null || typeof (obj) != 'object')
			return obj;
		var temp = new obj.constructor();
		for (var key in obj)
			temp[key] = this.clone(obj[key]);
		return temp;
	}


	//fromJSON(json) {
	//       for (var propName in json)
	//           this[propName] = json[propName];
	//       return this;
	//   }

	//toFormGroup(profile: Profile) {
	//	let group: any = {};



	//	//questions.forEach(question => {
	//	//	group[question.key] = question.required ? new FormControl(question.value || '', Validators.required)
	//	//		: new FormControl(question.value || '');
	//	//});
	//	return new FormGroup(group);
	//}
}