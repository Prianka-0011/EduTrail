import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LabRequestListComponent } from './lab-request-list.component';

describe('LabRequestListComponent', () => {
  let component: LabRequestListComponent;
  let fixture: ComponentFixture<LabRequestListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LabRequestListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LabRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
