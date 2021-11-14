import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse, HttpErrorResponse, HttpClient
} from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import {Router} from '@angular/router';
import { StravaAuth } from '../models/StravaAuth';

/** Pass untouched request through to the next request handler. */
@Injectable()
export class Interceptor implements HttpInterceptor {

  constructor(private router: Router, private http: HttpClient) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) : Observable<HttpEvent<any>> {
      if (!req.url.endsWith("authenticate")) {
        var token = localStorage.getItem("token");
        
        if (token) {
          req = req.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`,
              ContentType: "application/json"
            }
          });
        }
      }

      // if this is an oauth response from strava, set the code, and acquire a token
      var code = req.params.get("code");
      var payload = {
        code: code
      };

      if (code) {
        this.http.post<StravaAuth>("https://localhost:44307/auth/token", payload)
          .pipe(map(auth => {
            localStorage.setItem("oauth-token", auth.AccessToken);
            localStorage.setItem("oauth-refresh-token", auth.RefreshToken);
        }));
      }





      return next.handle(req).pipe( tap(() => {},
      (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status !== 401) {
         return;
        }
        this.router.navigate(['login']);
      }
    }));
  }

  refresh() {

  }

  getStravaOauthToken(code: string) {
    this.http.post<StravaAuth>("https://localhost:44307/auth/token", { code })
          .pipe(map(auth => {
            localStorage.setItem("oauth-token", auth.AccessToken);
            localStorage.setItem("oauth-refresh-token", auth.RefreshToken);
            localStorage.setItem("oauth-expiresAt", auth.ExpiresAt.toString());
        }));
  }
}