import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardCategoryAmountComponent } from './dashboard-category-amount.component';

describe('DashboardCategoryAmountComponent', () => {
  let component: DashboardCategoryAmountComponent;
  let fixture: ComponentFixture<DashboardCategoryAmountComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardCategoryAmountComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardCategoryAmountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
