import {
  AfterViewInit,
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  ElementRef,
  EventEmitter,
  Output,
  ViewChild
} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgIf} from '@angular/common';
import flatpickr from 'flatpickr';
import 'flatpickr/dist/themes/material_blue.css';
import {DateUtils} from '../../utils/date.utils';

@Component({
  selector: 'app-date-picker',
  imports: [
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class DatePickerComponent implements AfterViewInit {
  @ViewChild('dateRangeInput') dateRangeInput!: ElementRef;
  @ViewChild('filterForm') filterForm!: ElementRef;
  @Output() datesSelected = new EventEmitter<{ startDate: string | null; endDate: string | null }>();
  selectedDates: { startDate: string | null; endDate: string | null } = {
    startDate: null,
    endDate: null,
  };
  flatpickrInstance: any;

  ngAfterViewInit() {
    this.flatpickrInstance = flatpickr(this.dateRangeInput.nativeElement, {
      mode: 'range',
      dateFormat: 'Y-m-d',
      conjunction: ' to ',
      maxDate: DateUtils.getToday(),
      allowInput: true,
      // locale: Italian,
      onChange: (selectedDates, dateStr) => {
        const dates = dateStr.split(' to ');
        this.selectedDates = {
          startDate: dates[0] || null,
          endDate: dates[1] || null
        };
        this.datesSelected.emit(this.selectedDates);
      },
    });
  }

  reset() {
    if (!this.selectedDates.startDate || !this.selectedDates.endDate) {
      return;
    }

    this.flatpickrInstance.clear();
    this.selectedDates = { startDate: null, endDate: null };

    // Reset the radio buttons in the filter form
    const radioButtons = this.filterForm.nativeElement.querySelectorAll('input[type="radio"]');
    radioButtons.forEach((radio: HTMLInputElement) => {
      radio.checked = false;
    });

    this.datesSelected.emit(this.selectedDates);
  }

  applyFilter(filter: 'lastYear' | 'lastMonth' | 'lastWeek') {
    let startDate: string;
    switch (filter) {
      case 'lastYear':
        startDate = DateUtils.getFirstDayOfCurrentYear();
        break;
      case 'lastMonth':
        startDate = DateUtils.getFirstDayOfCurrentMonth();
        break;
      case 'lastWeek':
        startDate = DateUtils.getFirstDayOfCurrentWeek();
        break;
      default:
        startDate = DateUtils.getToday();
    }

    this.selectedDates = {
      startDate: startDate,
      endDate: DateUtils.getToday()
    };

    this.flatpickrInstance.setDate(
      [this.selectedDates.startDate, this.selectedDates.endDate]);

    this.datesSelected.emit(this.selectedDates);
  }
}
