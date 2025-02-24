import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./routes/home/home.component')
        .then(m => m.HomeComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./routes/dashboard/dashboard.component')
        .then(m => m.DashboardComponent)
  },
  {
    path: 'transactions',
    loadComponent: () =>
      import('./routes/transactions/transactions.component')
        .then(m => m.TransactionsComponent)
  },
  // {
  //   path: 'transactions/:id',
  //   loadComponent: () =>
  //     import('./routes/transaction-detail/transaction-detail.component')
  //       .then(m => m.TransactionDetailComponent)
  // },
  {
    path: 'import-transactions',
    loadComponent: () =>
      import('./routes/import-transactions/import-transactions.component')
        .then(m => m.ImportTransactionsComponent)
  },
  {
    path: '**',
    loadComponent: () =>
      import('./routes/not-found/not-found.component')
        .then(m => m.NotFoundComponent)
  }
];
