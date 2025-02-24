import {Injectable, inject} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {forkJoin, mergeMap, of} from 'rxjs';
import {exhaustMap, catchError} from 'rxjs/operators';
import {StatisticsService} from '../../services/statistics-service/statistics.service';
import {StatisticsActions} from './statistics.actions';

@Injectable()
export class StatisticsEffects {
  private actions$ = inject(Actions);
  private statisticsService = inject(StatisticsService);

  getAllStat = createEffect(() => {
    return this.actions$.pipe(
      ofType(StatisticsActions.getStatistics),
      exhaustMap((data) =>
        forkJoin({
          totalSpent: this.statisticsService.totalSpent(data.query),
          spentPerCategory: this.statisticsService.spentPerCategory(data.query),
        }).pipe(
          mergeMap(({ totalSpent, spentPerCategory }) => [
            StatisticsActions.setTotalSpent({ response: totalSpent }),
            StatisticsActions.setSpentPerCategory({ response: spentPerCategory }),
          ]),
          catchError((error: { message: string }) =>
            of(StatisticsActions.errorStatService({ error: error.message }))
          )
        )
      ))
  });
}
