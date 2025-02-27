import {inject, Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {forkJoin, mergeMap, of} from 'rxjs';
import {catchError, exhaustMap} from 'rxjs/operators';
import {StatisticsService} from '../../services/statistics-service/statistics.service';
import {StatisticsActions} from './statistics.actions';
import {DateUtils} from '../../shared/utils/date.utils';
import {Flow} from '../../constants/enums';

@Injectable()
export class StatisticsEffects {
  private actions$ = inject(Actions);
  private statisticsService = inject(StatisticsService);

  getMainStat = createEffect(() => {
    return this.actions$.pipe(
      ofType(StatisticsActions.getMainStatistics),
      exhaustMap((data) =>
        forkJoin({
          totalSpentWeek: this.statisticsService.totalAmount({
            fromDate: DateUtils.getFirstDayOfCurrentWeek(),
            toDate: DateUtils.getToday(),
            flow: Flow.Expense
          }),
          totalSpentMonth: this.statisticsService.totalAmount({
            fromDate: DateUtils.getFirstDayOfCurrentMonth(),
            toDate: DateUtils.getToday(),
            flow: Flow.Expense
          }),
          totalSpentYear: this.statisticsService.totalAmount({
            fromDate: DateUtils.getFirstDayOfCurrentYear(),
            toDate: DateUtils.getToday(),
            flow: Flow.Expense
          }),
          // totalEarned: this.statisticsService.totalEarned(data.query),
          // spentPerCategory: this.statisticsService.spentPerCategory(data.query),
        }).pipe(
          mergeMap(({totalSpentWeek, totalSpentMonth, totalSpentYear}) => [
            StatisticsActions.setMainStatistics({
              response: {
                totalSpentWeek: totalSpentWeek,
                totalSpentMonth: totalSpentMonth,
                totalSpentYear: totalSpentYear,
                totalEarnedMonth: 0,
                totalEarnedYear: 0
              }
            }),
            // StatisticsActions.setTotalEarned({ response: totalEarned }),
            // StatisticsActions.setSpentPerCategory({ response: spentPerCategory }),
          ]),
          catchError((error: { message: string }) =>
            of(StatisticsActions.errorStatService({error: error.message}))
          )
        )
      ))
  });
}
