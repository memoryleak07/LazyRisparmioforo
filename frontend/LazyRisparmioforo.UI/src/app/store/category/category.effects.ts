import {Injectable, inject} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {mergeMap, of} from 'rxjs';
import {exhaustMap, catchError} from 'rxjs/operators';
import {TransactionService} from '../../services/transaction-service/transaction.service';
import {CategoryActions} from './category.actions';
import {CategoryService} from '../../services/category-service/category.service';
import {TransactionActions} from '../transaction/transaction.actions';

@Injectable()
export class CategoryEffects {
  private actions$ = inject(Actions);
  private categoryService = inject(CategoryService);

  allCategories = createEffect(() => {
    return this.actions$.pipe(
      ofType(CategoryActions.allCategories),
      exhaustMap(() =>
        this.categoryService.all().pipe(
          mergeMap((response) => [
            CategoryActions.setCategories({response})
          ]),
          catchError((error: { message: string }) =>
            of(CategoryActions.errorCategoryService({error: error.message}))
          )
        )
      )
    );
  });
}
