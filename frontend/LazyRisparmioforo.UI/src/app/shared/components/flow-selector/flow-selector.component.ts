import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Flow } from '../../../constants/enums';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-flow-selector',
  imports: [NgIf],
  templateUrl: './flow-selector.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FlowSelectorComponent),
      multi: true,
    },
  ],
})
export class FlowSelectorComponent implements ControlValueAccessor {
  @Input() flow: Flow | undefined = undefined;
  @Input() hasResetButton: boolean = false;

  @Output() flowChange = new EventEmitter<Flow | undefined>();

  protected readonly Flow = Flow;

  private _selectedFlow: Flow | undefined = undefined;

  private _onChange: (value: Flow | undefined) => void = () => {};
  private _onTouched: () => void = () => {};

  get selectedFlow(): Flow | undefined {
    return this._selectedFlow;
  }

  set selectedFlow(value: Flow | undefined) {
    this._selectedFlow = value;
    this._onChange(value);
    this._onTouched();
    this.flowChange.emit(value);
  }

  onFlowChange(flow: Flow): void {
    this.selectedFlow = flow;
  }

  resetFlow(): void {
    this.selectedFlow = undefined;
  }

  writeValue(value: Flow | undefined): void {
    this.selectedFlow = value;
  }

  registerOnChange(fn: (value: Flow | undefined) => void): void {
    this._onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this._onTouched = fn;
  }
}
