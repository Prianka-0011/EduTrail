import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HelpRequestDashboardComponent } from './help-request-dashboard.component';

describe('HelpRequestDashboardComponent', () => {
  let component: HelpRequestDashboardComponent;
  let fixture: ComponentFixture<HelpRequestDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HelpRequestDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HelpRequestDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
