import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {selectSpentPerCategory} from '../../../store/statistics/statistics.reducers';
import {Store} from '@ngrx/store';
import {combineLatest, Subscription} from 'rxjs';
import {ChartComponent} from 'ng-apexcharts';
import {selectAllCategories} from '../../../store/category/category.reducers';

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

  constructor() {
  }

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

        console.log("series", series);
        console.log("categ", categories);
        this.chartOptions = {
          series: series,
          chart: {
            type: "donut"
          },
          labels: categories,
          responsive: [
            {
              breakpoint: 480,
              options: {
                chart: {
                  width: 200
                },
                legend: {
                  position: "bottom"
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
