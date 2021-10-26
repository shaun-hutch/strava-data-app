import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { reduceEachTrailingCommentRange } from 'typescript';
import { ActivityService } from '../services/activity.service';
import { LocationPoint } from '../models/Location';
import { listenerCount } from 'process';
const L = require('leaflet');

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, AfterViewInit {

  private map: any;

  private initMap(): void {
    this.map = L.map('map', {
      center: [ -36.9015866, 174.9387134 ],
      zoom: 15
    });

    const tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 18,
      minZoom: 3,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    });

    tiles.addTo(this.map);

    this.activityService.getActivities().subscribe((data) => {
      console.log("got data");
      console.log(data[0].Id);
      this.getPoints(data[0].Id).subscribe((data) => {

        console.log("got single activity");
        let pointList : any = [];
        data.forEach(element => {
          pointList.push(new L.LatLng(element.Latitude, element.Longitude));
        });

        var line = new L.Polyline(pointList, {
          color: 'red',
          weight: 3
        });

        line.addTo(this.map);
      });
      
      

    })

  }

  constructor(private activityService: ActivityService) { }

  ngOnInit(): void {

    this.activityService.getActivities().subscribe((data) => {
      console.log(data);
    });
  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  private getPoints(activityId: number) : Observable<LocationPoint[]> {
    return this.activityService.getPolyline(activityId);
  }

}
