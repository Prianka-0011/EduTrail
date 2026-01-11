import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionCreateOrUpdateComponent } from './question-create-or-update.component';

describe('QuestionCreateOrUpdateComponent', () => {
  let component: QuestionCreateOrUpdateComponent;
  let fixture: ComponentFixture<QuestionCreateOrUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuestionCreateOrUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuestionCreateOrUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
