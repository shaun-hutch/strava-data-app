import { Component, OnInit } from '@angular/core';
import { ActivityService } from '../services/activity.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  constructor(private activityService: ActivityService) { }

  ngOnInit(): void {

    this.activityService.getActivities().subscribe((data) => {
      console.log(data);
    });
  }

}
