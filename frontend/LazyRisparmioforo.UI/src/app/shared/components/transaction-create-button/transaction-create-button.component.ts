import { Component, ViewChild } from '@angular/core';
import {TransactionDialogComponent} from '../transaction-dialog/transaction-dialog.component';

@Component({
  selector: 'app-transaction-create-button',
  template: `
    <button (click)="openTransactionDialog()"
            class="btn btn-primary btn-sm">
      Create
    </button>

    <app-transaction-dialog #transactionDialog>
    </app-transaction-dialog>
  `,
  imports: [
    TransactionDialogComponent
  ]
})
export class TransactionCreateButtonComponent {
  @ViewChild('transactionDialog') transactionDialog!: TransactionDialogComponent;

  openTransactionDialog(): void {
    this.transactionDialog.open();
  }
}
