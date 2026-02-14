import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionTypeCreateOrUpdateComponent } from './question-type-create-or-update.component';

describe('QuestionTypeCreateOrUpdateComponent', () => {
  let component: QuestionTypeCreateOrUpdateComponent;
  let fixture: ComponentFixture<QuestionTypeCreateOrUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuestionTypeCreateOrUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuestionTypeCreateOrUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
