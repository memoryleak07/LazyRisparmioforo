import {Component, ElementRef, EventEmitter, Input, Output, ViewChild} from '@angular/core';
import {NgClass} from '@angular/common';

@Component({
  selector: 'app-dialog',
  imports: [
    NgClass
  ],
  templateUrl: './dialog.component.html'
})
export class DialogComponent {
  @Input() title: string = 'Default Title';
  @Input() content: string = 'Default content text.';
  @Input() submitButtonText: string = 'Submit';
  @Input() submitButtonColor: string = 'btn-primary';
  @Input() closeButtonText: string = 'Close';

  @Output() onClose = new EventEmitter<void>();
  @Output() onSubmit = new EventEmitter<void>();

  @ViewChild('dialog') dialogRef!: ElementRef<HTMLDialogElement>;

  open(): void {
    this.dialogRef.nativeElement.showModal();
  }

  close(): void {
    this.dialogRef.nativeElement.close();
  }

  handleSubmit(): void {
    this.onSubmit.emit();
    this.close();
  }

  handleCancel(): void {
    this.onClose.emit();
    this.close();
  }
}
