import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest
} from '@angular/common/http';

import { Observable } from 'rxjs';

/** Pass untouched request through to the next request handler. */
@Injectable()
export class Interceptor implements HttpInterceptor {

    token: string = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2MzQ3MjA0MDAsImV4cCI6MTYzNTMyNTIwMCwiaWF0IjoxNjM0NzIwNDAwfQ.mCXmBzUrY7xrYM0m9XPJG-n4NQhh3KiTyThClh5Nfhk"

  intercept(req: HttpRequest<any>, next: HttpHandler):
    Observable<HttpEvent<any>> {
        req.headers.append("Authorization", "Bearer " + this.token);

    return next.handle(req);
  }
}