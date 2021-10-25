import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Strava Run App';

  auth: AuthService;

  constructor(authService: AuthService) {
    this.auth = authService;
  }

  isLoggedIn(): boolean {
    var token = localStorage.getItem("token");
    return token == null ? false : true;
  }

  logout() {
    this.auth.logout();
  }
}