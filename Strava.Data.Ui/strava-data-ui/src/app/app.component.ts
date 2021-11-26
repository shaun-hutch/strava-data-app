import { Component, Input } from '@angular/core';
import { Globals } from './globals';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Strava Run App';

  auth: AuthService;
  @Input()
  oauthUrl: string;

  constructor(authService: AuthService) {
    this.auth = authService;
    this.oauthUrl = Globals.oauthUrl;
  }

  isLoggedIn(): boolean {
    var token = localStorage.getItem("token");
    return token == null ? false : true;
  }

  logout() {
    this.auth.logout();
  }
}