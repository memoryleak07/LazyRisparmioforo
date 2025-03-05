import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {Subscription} from 'rxjs';
import {Store} from '@ngrx/store';
import {selectMonthlySummary} from '../../../store/statistics/statistics.reducers';
import {ChartComponent} from 'ng-apexcharts';
import {DateUtils} from '../../utils/date.utils';
import {environment} from '../../../../environments/environment';
import {formatAmount} from '../../pipes/amount.pipe';


@Component({
  selector: 'app-bar-chart',
  imports: [
    ChartComponent
  ],
  templateUrl: './bar-chart.component.html',
  styleUrl: './bar-chart.component.css'
})
export class BarChartComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscription: Subscription = new Subscription();
  private series: any;
  public chartOptions: any;

  constructor() {}

  ngOnInit() {
    this.subscription.add(this.store.select(selectMonthlySummary).subscribe((data) => {
      const completeData = new Array(data.length + 1);
      for (let i = 1; i <= completeData.length; i++) {
        const existingMonth = data.find((item) => item.month === i);
        if (existingMonth) {
          completeData[i - 1] = existingMonth;
        } else {
          completeData[i - 1] = {month: i, income: 0, expense: 0, balance: 0};
        }
      }
      this.series = [
        {
          name: "Income",
          data: completeData.map((item) => item.income)
        },
        {
          name: "Expense",
          data: completeData.map((item) => item.expense * -1)
        },
        {
          name: "Balance",
          data: completeData.map((item) => item.balance)
        }
      ];

      this.chartOptions = {
        series: this.series,
        chart: {
          type: "bar",
          height: 350
        },
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: "55%",
          }
        },
        dataLabels: {
          enabled: false
        },
        stroke: {
          show: true,
          width: 2,
          colors: ["transparent"]
        },
        colors: ["var(--color-success)", "var(--color-error)", "var(--color-info)"],
        xaxis: {
          categories: DateUtils.MONTHS_SHORT.slice(0, data.length - 1),
        },
        yaxis: {
          title: {
            text: "€ (euro)"
          }
        },
        fill: {
          opacity: 1
        },
        tooltip: {
          y: {
            formatter: function (val: number) {
              return formatAmount(val);
            }
          }
        }
      };
    }));
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
