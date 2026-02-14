import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseOfferingCreateOrEditComponent } from './course-offering-create-or-edit.component';

describe('CourseOfferingCreateOrEditComponent', () => {
  let component: CourseOfferingCreateOrEditComponent;
  let fixture: ComponentFixture<CourseOfferingCreateOrEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseOfferingCreateOrEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseOfferingCreateOrEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
