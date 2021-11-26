import { Component, OnInit } from '@angular/core';
import { StravaAuth } from '../models/StravaAuth';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Globals } from '../globals';
@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})
export class CallbackComponent implements OnInit {

  constructor(private route: ActivatedRoute,  private router: Router, private http: HttpClient) { }

  ngOnInit(): void {

    var code = this.route.snapshot.queryParamMap.get('code') ?? undefined;
    console.log(code);

    // if this is an oauth response from strava, set the code, and acquire a token
    // var code = req.params.get("code");

    // if (code) {
    //   console.log("token");
    //   this.setStravaOauthToken(code);
    // }

    if (code) {
      this.setAuthToken(code).subscribe((data: StravaAuth) => {
        console.log("yee", data);
        localStorage.setItem("oauth-token", data.AccessToken);
        localStorage.setItem("oauth-refresh-token", data.RefreshToken);
        localStorage.setItem("oauth-expiresAt", data.ExpiresAt.toString());

        this.router.navigate(['']);


      });
    }
  }


  setAuthToken(code: string) : Observable<StravaAuth> {
    return this.http.post<StravaAuth>(`${Globals.apiUrl}auth/token`, { code: code, userId: 1 });
  }

  refresh(refreshToken: string) {
    this.http.post<StravaAuth>(`${Globals.apiUrl}/auth/refresh`, { refreshToken })
    .pipe(map(auth => {
      localStorage.setItem("oauth-token", auth.AccessToken);
      localStorage.setItem("oauth-refresh-token", auth.RefreshToken);
      localStorage.setItem("oauth-expiresAt", auth.ExpiresAt.toString());
    }));
  }
}
