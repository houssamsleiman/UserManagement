import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { UtilityService } from 'src/app/helper/utility.service';
import { UserService } from 'src/app/services/user.service';
import { takeWhile, filter, finalize, tap } from 'rxjs/operators';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit, OnDestroy {
  private isDestroyed = false;
  loading: boolean;
  isEditable: boolean = false;
  userList: any[];
  cols: any[];

  constructor(private userService: UserService,
    private utilityservice: UtilityService,
    private router: Router
  ) { }

  ngOnInit() {
    this.userService
      .getAll()
      .pipe(
        takeWhile(() => !this.isDestroyed),
        filter(response => !isNullOrUndefined(response)),
        finalize(() => this.loading = false),
        tap(response => {
          this.userList = response.users;
          if (response.isEditable) {
            this.isEditable = response.isEditable;
          }
        },
          error => {
            this.utilityservice.showError(error);

          })).subscribe();

    this.cols = [
      { field: 'id', header: 'Id' },
      { field: 'firstName', header: 'First Name' },
      { field: 'lastName', header: 'Last Name' },
      { field: 'userName', header: 'User Name' }
    ];
  }

  onRowEdit(row) {
    this.router.navigate([`UserEdit/${row.id}`]);
  }
  ngOnDestroy(): void {
    this.isDestroyed = true;
  }
}
