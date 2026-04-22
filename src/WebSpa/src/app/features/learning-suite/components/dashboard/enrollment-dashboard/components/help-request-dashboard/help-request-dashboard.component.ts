import { Component, OnInit } from '@angular/core';
import { ChatComponent } from '../../../../../../../chat/components/chat/chat.component';
import { SideDrawerComponent } from '../../../../../../../shared/components/side-drawer/side-drawer.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';
import { IWeeklyLabRequestDetail } from '../../../interfaces/IWeeklyLabRequestDetail';
import { HelpRequestDashboardService } from '../../../services/help-request-dashboard.service';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartType } from 'chart.js';
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);


@Component({
  selector: 'app-help-request-dashboard',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterModule,
    CommonModule,
    FormsModule,
    SideDrawerComponent,
    ChatComponent,
    BaseChartDirective,
  ],
  templateUrl: './help-request-dashboard.component.html',
  styleUrls: ['./help-request-dashboard.component.scss']
})
export class HelpRequestDashboardComponent implements OnInit {

  weeklyData: IWeeklyLabRequestDetail[] = [];
  loading = true;

  // Pie Chart
  pieChartLabels: string[] = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
  pieChartData: number[] = [];
  pieChartType: ChartType = 'pie';

  // Bar Chart
  barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Weekly Requests'
      }
    ]
  };

  constructor(private dashboardService: HelpRequestDashboardService) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard() {
    this.dashboardService.getWeeklyDashboard().subscribe({
      next: (res) => {
        this.weeklyData = res.weeklyDataList || [];
        this.prepareCharts();
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  prepareCharts() {
    const totals = this.weeklyData.reduce(
      (acc, w) => {
        acc[0] += w.monday;
        acc[1] += w.tuesday;
        acc[2] += w.wednesday;
        acc[3] += w.thursday;
        acc[4] += w.friday;
        acc[5] += w.saturday;
        acc[6] += w.sunday;
        return acc;
      },
      [0, 0, 0, 0, 0, 0, 0]
    );
    this.pieChartData = totals;

    this.barChartData.labels = this.weeklyData.map(w => w.week);
    this.barChartData.datasets[0].data = this.weeklyData.map(w => w.total);
  }

  getTotalRequests(): number {
    return this.weeklyData.reduce((sum, w) => sum + w.total, 0);
  }
}