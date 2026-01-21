import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateOrEditAssessmentComponent } from './create-or-edit-assessment.component';

describe('CreateOrEditAssessmentComponent', () => {
  let component: CreateOrEditAssessmentComponent;
  let fixture: ComponentFixture<CreateOrEditAssessmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateOrEditAssessmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateOrEditAssessmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
