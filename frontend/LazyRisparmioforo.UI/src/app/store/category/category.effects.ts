import {Injectable, inject} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {forkJoin, map, mergeMap, of} from 'rxjs';
import {exhaustMap, catchError} from 'rxjs/operators';
import {TransactionService} from '../../services/transaction-service/transaction.service';
import {CategoryActions} from './category.actions';
import {CategoryService} from '../../services/category-service/category.service';
import {TransactionActions} from '../transaction/transaction.actions';
import {DateUtils} from '../../shared/utils/date.utils';
import {StatisticsActions} from '../statistics/statistics.actions';
import {CATEGORY_CONFIG} from '../../constants/default-categories';

@Injectable()
export class CategoryEffects {
  private actions$ = inject(Actions);
  private categoryService = inject(CategoryService);

  allCategories = createEffect(() =>
    this.actions$.pipe(
      ofType(CategoryActions.allCategories),
      exhaustMap(() =>
        this.categoryService.all().pipe(
          map((categories) => {
            const mergedCategories = categories.map(category => {
              const defaultConfig = CATEGORY_CONFIG.find(cfg => cfg.id === category.id);
              return {
                ...category,
                icon: defaultConfig ? defaultConfig.icon : '',
                color: defaultConfig ? defaultConfig.color : ''
              };
            });
            return CategoryActions.setCategories({response: mergedCategories});
          }),
          catchError((error: { message: string }) =>
            of(CategoryActions.errorCategoryService({error: error.message}))
          )
        )
      )
    )
  );
}
