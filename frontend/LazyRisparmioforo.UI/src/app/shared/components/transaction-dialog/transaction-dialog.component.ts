import {Component, ElementRef, EventEmitter, inject, Input, OnDestroy, OnInit, Output, ViewChild} from '@angular/core';
import {Store} from '@ngrx/store';
import {Subscription} from 'rxjs';
import {selectAllCategories} from '../../../store/category/category.reducers';
import {Category} from '../../../services/category-service/category.model';
import {Flow} from '../../../constants/enums';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {TransactionActions} from '../../../store/transaction/transaction.actions';
import {
  selectCurrentTransaction,
  selectLastTransactions,
  selectMessage
} from '../../../store/transaction/transaction.reducers';
import {ToastService} from '../../ui/toast/toast.service';
import {NgForOf, NgIf} from '@angular/common';
import {FlowSelectorComponent} from '../flow-selector/flow-selector.component';
import {DialogComponent} from '../../ui/dialog/dialog.component';
import {Transaction} from '../../../services/transaction-service/transaction.model';
import {DateUtils} from '../../utils/date.utils';

@Component({
  selector: 'app-transaction-dialog',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgIf,
    FlowSelectorComponent,
    NgForOf,
    DialogComponent
  ],
  templateUrl: './transaction-dialog.component.html'
})
export class TransactionDialogComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private toastService = inject(ToastService);
  private subscriptions = new Subscription();
  public transaction: Transaction | null = null;
  public categories: Category[] = [];
  public formGroup: FormGroup;

  private _transactionId: number | undefined;

  @Input()
  set transactionId(value: number | undefined) {
    this._transactionId = value;
    if (value) {
      this.subscriptions.add(this.store.select(selectLastTransactions).subscribe((transactions) => {
        let transaction = transactions.find(x => x.id === value);
        if (!transaction) {
          return;
        }
        this.transaction = transaction;
        this.initializeFormWithData();
      }));
    }
  }
  get transactionId(): number | undefined {
    return this._transactionId;
  }

  @Output() onClose = new EventEmitter<void>();
  @Output() onSubmit = new EventEmitter<void>();

  @ViewChild('dialog_transaction') dialogRef!: ElementRef<HTMLDialogElement>;
  @ViewChild(DialogComponent) modal!: DialogComponent;

  constructor(private fb: FormBuilder) {
    this.formGroup = this.fb.group({
      flow: [Flow.Expense, [Validators.required]],
      date: ['', [Validators.required]],
      amount: ['', [Validators.required]],
      description: ['', [Validators.maxLength(4000)]],
      categoryId: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {
    if (this.transactionId) {
      this.subscriptions.add(this.store.select(selectCurrentTransaction).subscribe((transaction) => {
        this.transaction = transaction;
        this.initializeFormWithData();
      }));
    }
    else {
      this.formGroup.patchValue({
        date: DateUtils.getToday(),
      });
    }

    this.subscriptions.add(this.store.select(selectMessage).subscribe((message) => {
      if (message) { this.toastService.addToast(message, 'success'); }
    }));

    this.subscriptions.add(this.store.select(selectAllCategories).subscribe((categories) => {
      this.categories = categories;
    }));
    // Predict category
    // this.subscriptions.add(
    //   this.transactionForm.get('description')!.valueChanges.pipe(
    //     debounceTime(500), // Wait for user to stop typing
    //     filter(value => value && value.trim().length > 0 && value.trim().length > 2), // Ignore empty values
    //     switchMap(description => this.categoryService.predict({ input: description }))
    //   ).subscribe(response => {
    //     this.prediction = response;
    //   })
    // );
  }

  ngOnDestroy() {
    this.resetFormValues();
    this.subscriptions.unsubscribe();
  }

  open(): void {
    this.dialogRef.nativeElement.showModal();
  }

  close(): void {
    this.dialogRef.nativeElement.close();
  }

  save(): void {
    if (!this.formGroup.valid) {
      return;
    }

    let { flow, date, amount, description, categoryId } = this.formGroup.value;

    amount = flow === Flow.Income
      ? amount
      : amount * -1;

    if (this.transactionId) {
      this.store.dispatch(TransactionActions.updateTransaction({
        request: {
          id: this.transactionId,
          date: date,
          amount: amount,
          categoryId: categoryId,
          description: description,
        }
      }));
      console.log('Transaction updated.');
    }
    else {
      this.store.dispatch(TransactionActions.createTransaction({
        request: {
          date: date,
          amount: amount,
          categoryId: categoryId,
          description: description
        }
      }));
      console.log("Transaction created.");
    }

    this.closeDialog();
  }

  closeDialog(): void {
    this.resetFormValues();
    this.close();
  }

  confirmDelete(): void {
    this.modal.open();
  }

  delete(): void {
    if (!this.transaction){
      return;
    }

    this.store.dispatch(TransactionActions.deleteTransaction({
      query: {
        id: this.transaction.id
      }
    }));
    console.log("Transaction deleted.");

    this.closeDialog();
  }

  private resetFormValues(): void {
    this.formGroup.reset({
      flow: Flow.Expense,
      date: DateUtils.getToday(),
    });
  }

  private initializeFormWithData(): void {
    if (!this.transaction) {
      return;
    }

    this.formGroup.patchValue({
      flow: this.transaction.flow,
      date: this.transaction.registrationDate,
      amount: this.transaction.amount,
      categoryId: this.transaction.categoryId,
      description: this.transaction.description,
    });
  }
}
