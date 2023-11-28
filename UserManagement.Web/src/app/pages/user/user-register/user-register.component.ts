import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { takeWhile, filter, finalize, tap } from 'rxjs/operators';
import { UtilityService } from 'src/app/helper/utility.service';
import { SlimLoadingBarService } from 'ng2-slim-loading-bar';
import { User } from 'src/app/models/User';
import { isNullOrUndefined } from 'util';
import { NotificationService } from '../../../helper/notification.service';


@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.scss']
})
export class UserRegisterComponent implements OnInit, OnDestroy {
  private isDestroyed = false;
  userForm: FormGroup;
  user = new User();
  userId: number;

  constructor(private userService: UserService,
    private utilityservice: UtilityService,
    private loadingBar: SlimLoadingBarService,
    private userFB: FormBuilder,
    private notificationService: NotificationService) {
    this.createUserForm();
  }

  createUserForm() {
    this.userForm = this.userFB.group({
      FirstName: ['', Validators.required],
      LastName: ['', Validators.required],
      Username: ['', Validators.compose([Validators.required, Validators.email])],
      Email: ['', Validators.compose([Validators.required, Validators.email])],
      Password: ['', Validators.compose([Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%#^*?&])[A-Za-z\d$@$!%#^*?&].{8,}$')])],
      ConfirmPassword: ['', Validators.required],
      AcceptTerms: [false, Validators.requiredTrue]
    }, {
        validator: MustMatch('Password', 'ConfirmPassword')
      });
  }
  // convenience getter for easy access to form fields
  get f() { return this.userForm.controls; }

  ngOnInit() {
    this.loadingBar.start();
    this.loadingBar.complete();
  }
  RegisterUser() {
    this.loadingBar.start();
    this.userService.register(this.user)
      .pipe(
        takeWhile(() => !this.isDestroyed),
        filter(response => !isNullOrUndefined(response)),
        finalize(() => this.loadingBar.complete()),
        tap(response => {
          if (!isNullOrUndefined(response.userExist) && response.userExist) {
            this.utilityservice.showError("User is already exist.");
          }
          else {
            this.notificationService.success("User is Registered Successfully", "Success");
          }
        },
          error => {
            this.utilityservice.showError(error);

          })).subscribe();
  }
  ngOnDestroy(): void {
    this.isDestroyed = true;
  }
}

// custom validator to check that two fields match
export function MustMatch(controlName: string, matchingControlName: string) {
  return (formGroup: FormGroup) => {
    const control = formGroup.controls[controlName];
    const matchingControl = formGroup.controls[matchingControlName];

    if (matchingControl.errors && !matchingControl.errors.mustMatch) {
      // return if another validator has already found an error on the matchingControl
      return;
    }

    // set error on matchingControl if validation fails
    if (control.value !== matchingControl.value) {
      matchingControl.setErrors({ mustMatch: true });
    } else {
      matchingControl.setErrors(null);
    }
  }
}