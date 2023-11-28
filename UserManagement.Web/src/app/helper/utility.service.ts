import { Injectable } from '@angular/core';
import { throwError, Observable } from 'rxjs';
import { isNullOrUndefined } from 'util';
import { NotificationService } from './notification.service';

@Injectable({
	providedIn: 'root'
})
export class UtilityService {

	constructor(
		public _notificationService: NotificationService) { }


	handleErrorPromise = (error: any): Observable<any> => {
		if (error.status !== null && error.status === 401) {
			this._notificationService.error('Unauthorized');
			return throwError('Unauthorized');
		}
		else if (error.error !== null && error.error.text) {
			this._notificationService.error(error.error.text);
			return throwError(error.error.text);
		}
		else if (error.error) {
			const splitter = !isNullOrUndefined(error.error.split) ? error.error.split(';') : '';
			let myError = error.error;
			if (splitter.length > 1) {
				myError = splitter[0];
			}
			let errorMessageTranslated = myError;
			if (errorMessageTranslated === myError) {
				// not translated
				errorMessageTranslated = 'GenericErrorMessage';
			}
			this._notificationService.error(`${errorMessageTranslated}${splitter.length > 1 ? splitter[1] : ''}`);
			return throwError(myError);
		} else {
			try {
				error = JSON.parse(error._body);
			} catch (e) { }

			const errMsg = error.error ? error.error : error.errorMessage
				? error.errorMessage
				: error.message
					? error.message
					: error._body
						? error._body
						: error.status
							? `${error.status} - ${error.statusText}`
							: 'unknown server error';

			console.error(errMsg);
			return throwError(errMsg);
		}
	}
	showError(error: string) {
		this._notificationService.error(error,
			'Error');
	}
}
