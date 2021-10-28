import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
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

  @Input() 
  activities: Activity[];

  private line: any;

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
  }

  constructor(private activityService: ActivityService) { }

  ngOnInit(): void {
    // grab all activities here and add to table
    this.activityService.getActivities().subscribe((data : Activity[]) => {
      this.activities = data;
    });
  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  showRun(id: number, all: boolean = false) {
    if (this.line) {
      this.map.removeLayer(this.line);
    }

    if (all) {
      this.activities.forEach(a => {
        this.addPoints(a.Id, false);
      })
    }
    else {
      this.addPoints(id);
    }
  }

  private getPoints(activityId: number) : Observable<LocationPoint[]> {
    return this.activityService.getPolyline(activityId);
  }

  loadRun(id : number) {
    this.showRun(id);
  }

  addPoints(id: number, setBounds: boolean = true) {
    this.getPoints(id).subscribe((data) => {
      let pointList : any = [];
      data.forEach(element => {
        pointList.push(new L.LatLng(element.Latitude, element.Longitude));
      });

      this.line = new L.Polyline(pointList, {
        color: 'red',
        weight: 3
      });

      this.line.addTo(this.map);
      if (setBounds) {
        this.setBounds(data);
      }
    });
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
