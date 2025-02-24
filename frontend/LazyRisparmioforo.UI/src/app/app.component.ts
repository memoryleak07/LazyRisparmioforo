import {Component, inject, OnInit} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {NavbarComponent} from './shared/ui/navbar/navbar.component';
import {ToastComponent} from './shared/ui/toast/toast.component';
import {Store} from '@ngrx/store';
import {CategoryActions} from './store/category/category.actions';
import {errorCategoryService} from './store/category/category.reducers';
import {ToastService} from './shared/ui/toast/toast.service';
import {selectErrorTransactionService} from './store/transaction/transaction.reducers';
import {selectErrorStatService} from './store/statistics/statistics.reducers';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, ToastComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'LazyRisparmioforo.UI';
  private store = inject(Store);
  private toastService = inject(ToastService);

  constructor() {
    this.store.select(errorCategoryService).subscribe((error) => {
      if (error) { this.toastService.addToast(error, 'error'); }
    });
    this.store.select(selectErrorTransactionService).subscribe((error) => {
      if (error) { this.toastService.addToast(error, 'error'); }
    });
    this.store.select(selectErrorStatService).subscribe((error) => {
      if (error) { this.toastService.addToast(error, 'error'); }
    });
  }
  ngOnInit() {
    this.store.dispatch(CategoryActions.allCategories());
  }
}
