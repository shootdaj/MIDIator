import { IDropdownOption } from '../../models/domainModel';

export class DropdownOption implements IDropdownOption {
	constructor(public value: string, public label: string) { }
}