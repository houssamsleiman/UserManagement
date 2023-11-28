import { Injectable } from '@angular/core';
import { ToasterService } from 'angular2-toaster';

@Injectable({
	providedIn: 'root'
})
export class NotificationService {
	constructor(private toastr: ToasterService) { }

	public success = (body: string, title = 'Operation successful'): void => {
		this.toastr.pop({ body: body, title: title, type: 'success' });
	};

	public error = (body: string, title = 'An error occured'): void => {
		this.toastr.pop({ body: body, title: title, type: 'error' });
	};

	public warning = (body: string, title = 'Something went wrong'): void => {
		this.toastr.pop({ body: body, title: title, type: 'warning' });
	};

	public info = (messageKey: string, titleKey: string = 'notifications.NoticeTitle'): void => {
		this.showMessage(messageKey, titleKey, 'info');
	};

	public completedSuccessfully = (
		messageKey: string = 'notifications.SuccessMessage',
		titleKey: string = 'notifications.SuccessTitle'
	): void => {
		this.showMessage(messageKey, titleKey, 'success');
	};

	public failure = (
		messageKey: string = 'notifications.ErrorMessage',
		titleKey: string = 'notifications.ErrorTitle'
	): void => {
		this.showMessage(messageKey, titleKey, 'error');
	};

	private showMessage(messageKey: string, titleKey: string, messageType: string): void {
		// this.translateService.get([messageKey, titleKey]).pipe(take(1))
		// .subscribe(data => {
		// this.toastr.pop({ body: data[messageKey], title: data[titleKey], type: messageType });
		this.toastr.pop({ body: messageKey, title: titleKey, type: messageType });
		// });
	}
}
