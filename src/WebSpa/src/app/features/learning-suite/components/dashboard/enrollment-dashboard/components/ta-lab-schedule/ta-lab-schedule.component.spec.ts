import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaLabScheduleComponent } from './ta-lab-schedule.component';

describe('TaLabScheduleComponent', () => {
  let component: TaLabScheduleComponent;
  let fixture: ComponentFixture<TaLabScheduleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaLabScheduleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaLabScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
