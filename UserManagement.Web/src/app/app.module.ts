import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { SlimLoadingBarModule } from 'ng2-slim-loading-bar';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from './pages/shared/header/header.component';
import { FooterComponent } from './pages/shared/footer/footer.component';
import { ToasterModule } from 'angular2-toaster';
import { ConfigurationService } from './helper/configuration.service';
import { AlertComponent } from './pages/alert/alert.component';
import { JwtInterceptor } from './helper/jwt.interceptor';
import { ErrorInterceptor } from './helper/error.interceptor';
import { TreeModule, DropdownModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { UserListComponent } from './pages/user/user-list/user-list.component';
import { UserEditComponent } from './pages/user/user-edit/user-edit.component';
import { UserRegisterComponent } from './pages/user/user-register/user-register.component';
import { UserLoginComponent } from './pages/user/user-login/user-login.component';

const initilizeConfiguration = (appConfig: ConfigurationService) => {
  return () => {
    return appConfig.loadConfiguration();
  }
};

const services = [
  ConfigurationService,
  {
    provide: APP_INITIALIZER,
    useFactory: initilizeConfiguration,
    multi: true,
    deps: [ConfigurationService]
  },
  { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }

];
@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    AlertComponent,
    UserListComponent,
    UserEditComponent,
    UserRegisterComponent,
    UserLoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    SlimLoadingBarModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ToasterModule.forRoot(),
    TreeModule,
    DragDropModule,
    DropdownModule,
    TableModule
  ],
  providers: [services],
  bootstrap: [AppComponent]
})
export class AppModule { }
