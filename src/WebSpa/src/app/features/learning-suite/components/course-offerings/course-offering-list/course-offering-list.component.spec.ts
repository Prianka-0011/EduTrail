import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseOfferingListComponent } from './course-offering-list.component';

describe('CourseOfferingListComponent', () => {
  let component: CourseOfferingListComponent;
  let fixture: ComponentFixture<CourseOfferingListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseOfferingListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseOfferingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
