import {Component, ElementRef, inject, OnDestroy, ViewChild} from '@angular/core';
import {ToastService} from '../../shared/ui/toast/toast.service';
import {Store} from '@ngrx/store';
import {Subscription} from 'rxjs';
import {ImportService} from '../../services/import-service/import.service';
import {TransactionActions} from '../../store/transaction/transaction.actions';

@Component({
  selector: 'app-import-transactions',
  imports: [],
  templateUrl: './import-transactions.component.html'
})
export class ImportTransactionsComponent implements OnDestroy {
  private importService = inject(ImportService);
  private toastService = inject(ToastService);
  private store = inject(Store);
  private subscriptions = new Subscription();
  private allowedTypes = ['text/csv'];
  public selectedFile: File | null = null;
  public isUploading = false;

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (file && this.allowedTypes.includes(file.type)) {
      this.selectedFile = file;
    } else {
      input.value = '';
      this.selectedFile = null;
      this.toastService.addToast('Please select a valid CSV file.', 'error')
    }
  }

  onUpload(): void {
    if (!this.selectedFile) {
      this.toastService.addToast('Please select a file first.', 'error');
      return;
    }

    this.isUploading = true;

    this.importService.csv(this.selectedFile).subscribe({
      next: (event) => {
      },
      error: (e) => {
        console.error("Upload failed: ", e);
        this.isUploading = false;
        this.toastService.addToast('Upload failed.', 'error');
      },
      complete: () => {
        console.info('Upload completed.');
        this.isUploading = false;
        this.toastService.addToast('Upload completed.', 'success');

        this.store.dispatch(TransactionActions.searchTransactions({
          query: {
            pageIndex: 0,
            pageSize: 5,
          }
        }));
      }
    });

    // Reset the input
    this.selectedFile = null;
    this.fileInput.nativeElement.value = '';
  }
}
