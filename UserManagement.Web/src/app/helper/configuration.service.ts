import { Injectable, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ConfigurationService {
	private configuration: any;

	constructor(private injector: Injector) { }

	async loadConfiguration() {

		let http = this.injector.get(HttpClient);
		const data = await http.get('../assets/config/config.json').toPromise();
		this.configuration = Object.assign({}, data);
	}

	get baseUrl() {
		return this.configuration.baseUrl;
	}

	get dateFormat() {
		return this.configuration.dateFormat;
	}

	get pCalendarDateFormat() {
		return this.configuration.pCalendarDateFormat;
	}

	get dateTimeFormat() {
		return this.configuration.dateTimeFormat;
	}

	get datePipeCulture() {
		return this.configuration.datePipeCulture;
	}
}
