import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseOfferingByUserComponent } from './course-offering-by-user.component';

describe('CourseOfferingByUserComponent', () => {
  let component: CourseOfferingByUserComponent;
  let fixture: ComponentFixture<CourseOfferingByUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseOfferingByUserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseOfferingByUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
