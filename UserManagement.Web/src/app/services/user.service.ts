import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/User';
import { ConfigurationService } from '../helper/configuration.service';
import { catchError } from 'rxjs/operators';
import { UtilityService } from '../helper/utility.service';
import { UserWithType } from '../models/userwithType';

@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient,
      private configurationService: ConfigurationService, 
      private utilityService: UtilityService) { }

    getAll() {
        return this.http.get<UserWithType>(`${this.configurationService.baseUrl}/api/users/GetAll`)
        .pipe(catchError(this.utilityService.handleErrorPromise));
    }
    GetById(id: number) {
        return this.http.get<User>(`${this.configurationService.baseUrl}/api/users/GetById?id=${id}`)
        .pipe(catchError(this.utilityService.handleErrorPromise));
    }
    SaveUserStatus(id: number,user: User) {
        return this.http.post(`${this.configurationService.baseUrl}/api/users/SaveUserStatus?id=${id}`, user)
        .pipe(catchError(this.utilityService.handleErrorPromise));
    }

    register(user: User) {
        return this.http.post(`${this.configurationService.baseUrl}/api/users/Register?`, JSON.stringify(user))
        .pipe(catchError(this.utilityService.handleErrorPromise));
    }

    delete(id: number) {
        return this.http.delete(`${this.configurationService.baseUrl}/api/users/${id}`)
        .pipe(catchError(this.utilityService.handleErrorPromise));
    }
}