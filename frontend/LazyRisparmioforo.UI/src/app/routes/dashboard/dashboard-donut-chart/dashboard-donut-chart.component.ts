import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {selectSpentPerCategory} from '../../../store/statistics/statistics.reducers';
import {Store} from '@ngrx/store';
import {combineLatest, Subscription} from 'rxjs';
import {ChartComponent} from 'ng-apexcharts';
import {selectAllCategories} from '../../../store/category/category.reducers';
import {formatAmount} from '../../../shared/pipes/amount.pipe';

@Component({
  selector: 'app-dashboard-donut-chart',
  imports: [
    ChartComponent
  ],
  templateUrl: './dashboard-donut-chart.component.html'
})
export class DashboardDonutChartComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscriptions: Subscription = new Subscription();
  public chartOptions: any;

  constructor() {}

  ngOnInit() {
    this.subscriptions.add(
      combineLatest([
        this.store.select(selectSpentPerCategory),
        this.store.select(selectAllCategories)
      ]).subscribe(([spentPerCategory, allCategories]) => {
        const series: number[] = spentPerCategory.map((item) => Math.abs(item.amount));

        const categories: string[] = spentPerCategory.map((item) => {
          const category = allCategories.find((cat) => cat.id === item.categoryId);
          return category ? category.name : "Unknown";
        });

        const colors: string[] = spentPerCategory.map((item) => {
          const category = allCategories.find((cat) => cat.id === item.categoryId);
          return category ? category.color : "#CCCCCC";
        });

        this.chartOptions = {
          series: series,
          chart: {
            type: "donut",
            width: "100%",
            height: "auto",
          },
          labels: categories,
          colors: colors,
          stroke: {
            show: true,
            width: 1,
            colors: ["var(--color-primary)"],
          },
          fill: {
            type: "gradient"
          },
          tooltip: {
            y: {
              formatter: function (val: number) {
                return formatAmount(val);
              },
            },
          },
          legend: {
            fontSize: "12px",
            fontFamily: "inherit",
            position: "bottom",
            horizontalAlign: "left",
            offsetY: 0,
            formatter: function(val: any, opts: any) {
              return val + " - " + formatAmount(opts.w.globals.series[opts.seriesIndex]) + " ";
            }
          },
          responsive: [
            {
              breakpoint: 480,
              options: {
                chart: {
                  width: "100%",
                },
                legend: {
                  position: "bottom",
                }
              }
            }
          ]
        };
      }),
    );
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }
}
