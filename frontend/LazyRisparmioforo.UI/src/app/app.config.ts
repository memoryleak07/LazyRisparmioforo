import { ApplicationConfig, provideZoneChangeDetection, isDevMode } from '@angular/core';
import {provideRouter, withComponentInputBinding} from '@angular/router';
import { provideStore, provideState } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {TransactionEffects} from './store/transaction/transaction.effects';
import {transactionFeatureKey, transactionReducer} from './store/transaction/transaction.reducers';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import {CategoryEffects} from './store/category/category.effects';
import {categoryFeatureKey, categoryReducer} from './store/category/category.reducers';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import {statisticsFeatureKey, statisticsReducer} from './store/statistics/statistics.reducers';
import {StatisticsEffects} from './store/statistics/statistics.effects';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withComponentInputBinding()),
    provideAnimationsAsync(),
    // provideHttpClient(withInterceptors([authInterceptor()])),
    provideHttpClient(),
    provideStore(),
    provideState({ name: transactionFeatureKey, reducer: transactionReducer }),
    provideState({ name: categoryFeatureKey, reducer: categoryReducer }),
    provideState({ name: statisticsFeatureKey, reducer: statisticsReducer }),
    provideEffects(TransactionEffects, CategoryEffects, StatisticsEffects),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() })
  ]
};
