import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router'
import { User } from '../models/User';

@Injectable({ providedIn: 'root' })
export class AuthService {
    constructor(private http: HttpClient, private router: Router) {}

    login(username: string, password: string) {
        return this.http.post<User>("https://localhost:44307/users/authenticate", { username, password })
            .pipe(map(user => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify(user));
                localStorage.setItem('token', user.Token);

                return user;
            }));
    }

    logout() {
        // remove user from local storage and set current user to null
        localStorage.removeItem('currentUser');
        localStorage.removeItem('token');
        this.router.navigate(['login']);
    }
}