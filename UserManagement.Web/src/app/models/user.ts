import { UserRole } from './userrole';

export class User {
    id: number;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    password: string;
    confirmPassword: string; 
    token: string;
    status: number; 
    userRole:UserRole[];
    constructor(){
        this.id=0;
        this.firstName= '';
        this.lastName= '';
        this.userName= '';
        this.email= '';
        this.password= '';  
        this.confirmPassword= '';  
    }
}


 