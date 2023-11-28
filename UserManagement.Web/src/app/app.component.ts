import { Component } from '@angular/core';
import { SlimLoadingBarService } from 'ng2-slim-loading-bar';
import {
  NavigationCancel,
  Event,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router
} from '@angular/router';
import { ToasterService, ToasterConfig } from 'angular2-toaster';


import { AuthenticationService } from './services/authentication.service';
import { User } from './models/User';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  currentUser: User;
  title = 'My Account Application';
  toastrConfig: ToasterConfig;

  constructor(private loadingBar: SlimLoadingBarService, private router: Router,
    private authenticationService: AuthenticationService
  ) {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
    this.router.events.subscribe((event: Event) => {
      this.navigationInterceptor(event);
    });
    this.toastrConfig = new ToasterConfig({
      newestOnTop: true,
      showCloseButton: true,
      tapToDismiss: false,
      preventDuplicates: true,
      mouseoverTimerStop: true
    });
  }


  private navigationInterceptor(event: Event): void {
    if (event instanceof NavigationStart) {
      this.loadingBar.start();
    }
    if (event instanceof NavigationEnd) {
      this.loadingBar.complete();
    }
    if (event instanceof NavigationCancel) {
      this.loadingBar.stop();
    }
    if (event instanceof NavigationError) {
      this.loadingBar.stop();
    }
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }
}
