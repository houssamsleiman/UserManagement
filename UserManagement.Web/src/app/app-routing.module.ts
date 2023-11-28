import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './helper/auth.guard';
import { UserListComponent } from './pages/user/user-list/user-list.component';
import { UserEditComponent } from './pages/user/user-edit/user-edit.component';
import { UserRegisterComponent } from './pages/user/user-register/user-register.component';
import { UserLoginComponent } from './pages/user/user-login/user-login.component';

const routes: Routes = [

  { path: '', component: UserListComponent, canActivate: [AuthGuard] },
  { path: 'login', component: UserLoginComponent },
  { path: 'userRegister', component: UserRegisterComponent },

  {
    canActivate: [AuthGuard],
    path: 'UserList',
    component: UserListComponent
  },
  {
    canActivate: [AuthGuard],
    path: 'UserEdit/:id',
    component: UserEditComponent
  },
  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
