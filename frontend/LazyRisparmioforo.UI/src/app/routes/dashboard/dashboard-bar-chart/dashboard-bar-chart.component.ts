import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {Subscription} from 'rxjs';
import {Store} from '@ngrx/store';
import {selectMonthlySummary} from '../../../store/statistics/statistics.reducers';
import {ChartComponent} from 'ng-apexcharts';
import {DateUtils} from '../../../shared/utils/date.utils';
import {environment} from '../../../../environments/environment';
import {formatAmount} from '../../../shared/pipes/amount.pipe';


@Component({
  selector: 'app-dashboard-bar-chart',
  imports: [
    ChartComponent
  ],
  templateUrl: './dashboard-bar-chart.component.html'
})
export class DashboardBarChartComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscription: Subscription = new Subscription();
  public chartOptions: any;

  constructor() {}

  ngOnInit() {
    this.subscription.add(
      this.store.select(selectMonthlySummary).subscribe((data) => {
        const series = [
          {
            name: "Income",
            data: data.map((item) => item.income),
          },
          {
            name: "Expense",
            data: data.map((item) => Math.abs(item.expense)),
          },
          {
            name: "Balance",
            data: data.map((item) => Math.abs(item.balance)),
          },
        ];

        // console.log("data", data);
        // console.log("series", this.series);

        this.chartOptions = {
          series: series,
          chart: {
            type: "bar",
            height: 350,
          },
          plotOptions: {
            bar: {
              horizontal: false,
              columnWidth: "55%",
            },
          },
          dataLabels: {
            enabled: false,
          },
          stroke: {
            show: true,
            width: 1,
            colors: ["var(--color-primary)"],
          },
          colors: ["var(--color-success)", "var(--color-error)", "var(--color-info)"],
          xaxis: {
            categories: DateUtils.MONTHS_SHORT.slice(0, series.length),
            labels: {
              style: {
                colors: "var(--color-base-content)",
                // fontSize: "14px",
                // fontWeight: 500,
              }
            },
          },
          yaxis: {
          },
          fill: {
            opacity: 1,
          },
          tooltip: {
            y: {
              formatter: function (val: number) {
                return formatAmount(val);
              },
            },
          },
        };
      })
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
