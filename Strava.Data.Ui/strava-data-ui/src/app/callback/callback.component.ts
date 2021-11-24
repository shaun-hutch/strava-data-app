import { Component, OnInit } from '@angular/core';
import { StravaAuth } from '../models/StravaAuth';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})
export class CallbackComponent implements OnInit {

  constructor(private route: ActivatedRoute,  private router: Router, private http: HttpClient) { }

  ngOnInit(): void {

    var code = this.route.snapshot.queryParamMap.get('code') ?? undefined;


    // if this is an oauth response from strava, set the code, and acquire a token
    // var code = req.params.get("code");

    // if (code) {
    //   console.log("token");
    //   this.setStravaOauthToken(code);
    // }

    if (code) {
      this.setAuthToken(code).subscribe((data: StravaAuth) => {
        console.log("yee", data);
      });
    }
  }


  setAuthToken(code: string) : Observable<StravaAuth> {
    return this.http.post<StravaAuth>("https://localhost:44307/auth/token", code);
  }

  refresh(refreshToken: string) {
    this.http.post<StravaAuth>("https://localhost:44307/auth/refresh", { refreshToken })
    .pipe(map(auth => {
      localStorage.setItem("oauth-token", auth.AccessToken);
      localStorage.setItem("oauth-refresh-token", auth.RefreshToken);
      localStorage.setItem("oauth-expiresAt", auth.ExpiresAt.toString());
    }));
  }

  setStravaOauthToken(code: string) {
    this.http.post<StravaAuth>("https://localhost:44307/auth/token", { code })
          .pipe(map(auth => {

            console.log("setting token");
            console.log(auth);

            localStorage.setItem("oauth-token", auth.AccessToken);
            localStorage.setItem("oauth-refresh-token", auth.RefreshToken);
            localStorage.setItem("oauth-expiresAt", auth.ExpiresAt.toString());
        }));
  }

}
