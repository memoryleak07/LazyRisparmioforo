import { createAction, props } from '@ngrx/store';
import {
  StatMainResponse,
  StatRequest,
  CategoryAmountResponse
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

  // static readonly clearStats = createAction(
  //   '[Statistics] ClearStats'
  // );

  static readonly errorStatService = createAction(
    '[Statistics] Error',
    props<{ error: string }>()
  );
}
