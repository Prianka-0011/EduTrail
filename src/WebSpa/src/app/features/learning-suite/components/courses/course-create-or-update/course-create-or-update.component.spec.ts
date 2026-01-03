import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseCreateOrUpdateComponent } from './course-create-or-update.component';

describe('CourseCreateOrUpdateComponent', () => {
  let component: CourseCreateOrUpdateComponent;
  let fixture: ComponentFixture<CourseCreateOrUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseCreateOrUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseCreateOrUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
