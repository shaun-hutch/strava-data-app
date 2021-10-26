import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { Activity } from '../models/Activity';
import { LocationPoint } from '../models/Location';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  constructor(private http: HttpClient) { }

  getActivities() : Observable<Activity[]> {
    return this.http.get<Activity[]>("https://localhost:44307/activities");
  }

  getActivity(id: number) : Observable<Activity> {
    return this.http.get<Activity>(`https://localhost:44307/activities/${id}`);
  }

  getPolyline(id: number) : Observable<LocationPoint[]> {
    return this.http.get<LocationPoint[]>(`https://localhost:44307/activities/${id}/polyline`);
  }
}
