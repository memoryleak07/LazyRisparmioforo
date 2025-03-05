import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardBalanceComponent } from './dashboard-balance.component';

describe('DashboardBalanceComponent', () => {
  let component: DashboardBalanceComponent;
  let fixture: ComponentFixture<DashboardBalanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardBalanceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardBalanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
