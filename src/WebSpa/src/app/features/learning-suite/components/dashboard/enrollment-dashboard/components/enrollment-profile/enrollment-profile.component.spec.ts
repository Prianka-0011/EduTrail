import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnrollmentProfileComponent } from './enrollment-profile.component';

describe('EnrollmentProfileComponent', () => {
  let component: EnrollmentProfileComponent;
  let fixture: ComponentFixture<EnrollmentProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EnrollmentProfileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EnrollmentProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
