import {inject, Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {forkJoin, mergeMap, of} from 'rxjs';
import {catchError, exhaustMap} from 'rxjs/operators';
import {StatisticsService} from '../../services/statistics-service/statistics.service';
import {StatisticsActions} from './statistics.actions';
import {DateUtils} from '../../shared/utils/date.utils';

@Injectable()
export class StatisticsEffects {
  private actions$ = inject(Actions);
  private statisticsService = inject(StatisticsService);

  getMainStat = createEffect(() => {
    return this.actions$.pipe(
      ofType(StatisticsActions.getMainStatistics),
      exhaustMap(() =>
        forkJoin({
          weekly: this.statisticsService.summary({
            fromDate: DateUtils.getFirstDayOfCurrentWeek(),
            toDate: DateUtils.getToday()
          }),
          monthly: this.statisticsService.summary({
            fromDate: DateUtils.getFirstDayOfCurrentMonth(),
            toDate: DateUtils.getToday()
          }),
          yearly: this.statisticsService.summary({
            fromDate: DateUtils.getFirstDayOfCurrentYear(),
            toDate: DateUtils.getToday()
          }),
          // Categories for current year
          spentPerCategory: this.statisticsService.spentPerCategory({
            fromDate: DateUtils.getFirstDayOfCurrentYear(),
            toDate: DateUtils.getToday()
          }),
        }).pipe(
          mergeMap(({weekly, monthly, yearly, spentPerCategory}) => [
            StatisticsActions.setMainStatistics({
              response: {
                weekly: weekly,
                monthly: monthly,
                yearly: yearly,
              }
            }),
            StatisticsActions.setSpentPerCategory({ response: spentPerCategory }),
          ]),
          catchError((error: { message: string }) =>
            of(StatisticsActions.errorStatService({error: error.message}))
          )
        )
      ))
  });
}
