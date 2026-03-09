import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmitHelpRequestComponent } from './submit-help-request.component';

describe('SubmitHelpRequestComponent', () => {
  let component: SubmitHelpRequestComponent;
  let fixture: ComponentFixture<SubmitHelpRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubmitHelpRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubmitHelpRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
