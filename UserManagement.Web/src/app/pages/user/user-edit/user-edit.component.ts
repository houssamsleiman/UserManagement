import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { UtilityService } from 'src/app/helper/utility.service';
import { takeWhile, filter, finalize, tap } from 'rxjs/operators';
import { isNullOrUndefined } from 'util';
import { SlimLoadingBarService } from 'ng2-slim-loading-bar';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss']
})
export class UserEditComponent implements OnInit, OnDestroy {
  private isDestroyed = false;
  angForm: FormGroup;
  user: any = {};
  userId: number;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private utilityservice: UtilityService,
    private loadingBar: SlimLoadingBarService,
    private fb: FormBuilder) {
    this.createForm();
  }

  createForm() {
    this.angForm = this.fb.group({
      FirstName: ['', Validators.required],
      LastName: ['', Validators.required],
      UserName: ['', Validators.required],
      Status: ['', Validators.compose([Validators.required, Validators.min(1), Validators.max(2)])]
    });
  }

  ngOnInit() {
    this.loadingBar.start();
    this.route.params.subscribe(params => {
      this.userId = Number(params['id']);
      this.userService.GetById(this.userId).subscribe(res => {
        this.user = res;
        this.loadingBar.complete();
      });
    });
  }
  updateUser() {
    this.loadingBar.start();
    this.userService.SaveUserStatus(this.userId, this.user)
      .pipe(
        takeWhile(() => !this.isDestroyed),
        filter(response => !isNullOrUndefined(response)),
        finalize(() => this.loadingBar.complete()),
        tap(response => {
          this.user = response;
          this.router.navigate(['UserList']);
        },
          error => {
            this.utilityservice.showError(error);

          })).subscribe();
  }
  ngOnDestroy(): void {
    this.isDestroyed = true;
  }

}

