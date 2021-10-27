import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ActivityService } from '../services/activity.service';
import { LocationPoint } from '../models/Location';
import { Activity } from '../models/Activity';
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
      center: [ -40.3373129,173.9639123 ],
      zoom: 3
    });

    const tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 18,
      minZoom: 3,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    });

    tiles.addTo(this.map);

    this.activityService.getActivities().subscribe((data : Activity[]) => {
      this.getPoints(data[0].Id).subscribe((data) => {
        let pointList : any = [];
        data.forEach(element => {
          pointList.push(new L.LatLng(element.Latitude, element.Longitude));
        });

        var line = new L.Polyline(pointList, {
          color: 'red',
          weight: 3
        });

        line.addTo(this.map);
        this.setBounds(data);
      });
    });

  }

  constructor(private activityService: ActivityService) { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  private getPoints(activityId: number) : Observable<LocationPoint[]> {
    return this.activityService.getPolyline(activityId);
  }


  private setBounds(points : LocationPoint[]) {
    let minLat = 180;
    let maxLat = -180; 
    let minLng = 180; 
    let maxLng = -180;
  
    points.forEach(p => {
      if (p.Latitude < minLat)
        minLat = p.Latitude;
      if (p.Latitude > maxLat)
        maxLat = p.Latitude;

      if (p.Longitude < minLng)
        minLng = p.Longitude;
      if (p.Longitude > maxLng)
        maxLng = p.Longitude;

    });    

    this.map.fitBounds([
      [minLat, minLng],
      [maxLat, maxLng]
    ],
    {
      duration: 1,
      easeLinearity: 0.5
    });
    
  }

}
