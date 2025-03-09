import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {Subscription} from 'rxjs';
import {Store} from '@ngrx/store';
import {selectMonthlySummary} from '../../../store/statistics/statistics.reducers';
import {ChartComponent} from 'ng-apexcharts';
import {DateUtils} from '../../../shared/utils/date.utils';
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


        this.chartOptions = {
          series: [
            {
              name: "Income",
              type: "column",
              data: data.map((item) => item.income),
            },
            {
              name: "Expense",
              type: "column",
              data: data.map((item) => Math.abs(item.expense)),
            },
            {
              name: "Balance",
              type: "area",
              data: data.map((item) => item.balance),
            }
          ],
          chart: {
            type: "line",
            width: "100%",
            height: "auto",
            zoom: {
              enabled: false
            }
          },
          stroke: {
            show: true,
            width: [1, 1, 2.5],
            curve: "smooth",
            colors: ["var(--color-success)", "var(--color-error)", "var(--color-info)"],
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
          fill: {
            opacity: [0.9, 0.7, 0.2],
            type: ["gradient", "gradient", "gradient"],
            gradient: {
              shade: "light",
              type: "vertical",
              shadeIntensity: 0.5,
              gradientToColors: ["var(--color-success)", "var(--color-error)", "var(--color-info)"], // Balance color
              opacityFrom: 0.9,
              opacityTo: 0.2,
              stops: [0, 100]
            }
          },
          colors: ["var(--color-success)", "var(--color-error)", "var(--color-info)"],
          xaxis: {
            categories: DateUtils.MONTHS_SHORT.slice(0, data.length),
            labels: {
              style: {
                colors: "var(--color-base-content)",
              }
            },
          },
          yaxis: {
            title: {
              text: "Euro €",
              style: {
                color: "var(--color-primary)",
              }
            },
          },
          tooltip: {
            shared: true,
            intersect: false,
            y: {
              formatter: function (val: number) {
                return formatAmount(val);
              },
            },
          },
          responsive: [
            {
              breakpoint: 768, // Tablets and below
              options: {
                chart: {
                  height: 300,
                  zoom: {
                    enabled: false
                  }
                },
                plotOptions: {
                  bar: {
                    columnWidth: "70%",
                  },
                },
              },
            },
            {
              breakpoint: 480, // Mobile screens
              options: {
                chart: {
                  height: 250,
                  zoom: {
                    enabled: false
                  }
                },
                stroke: {
                  width: [0, 0, 2], // Thinner lines
                },
              },
            },
          ],
        };
      })
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
