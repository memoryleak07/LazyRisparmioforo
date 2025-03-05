import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardDonutChartComponent } from './dashboard-donut-chart.component';

describe('DashboardDonutChartComponent', () => {
  let component: DashboardDonutChartComponent;
  let fixture: ComponentFixture<DashboardDonutChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardDonutChartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardDonutChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
