import { createAction, props } from '@ngrx/store';
import {
  StatMainResponse,
  StatRequest,
  CategoryAmountResponse, MonthlySummaryResponse
} from '../../services/statistics-service/statistics.models';

export class StatisticsActions {

  static readonly getMainStatistics = createAction(
    '[Statistics] GetMainStatistics'
  );

  static readonly setMainStatistics = createAction(
    '[Statistics] SetMainStatistics',
    props<{ response: StatMainResponse }>()
  );

  static readonly setSpentPerCategory = createAction(
    '[Statistics] SetSpentPerCategory',
    props<{ response: CategoryAmountResponse[] }>()
  );

  static readonly setMonthlySummary = createAction(
    '[Statistics] SetMonthlySummary',
    props<{ response: MonthlySummaryResponse[] }>()
  );

  static readonly errorStatService = createAction(
    '[Statistics] Error',
    props<{ error: string }>()
  );
}
