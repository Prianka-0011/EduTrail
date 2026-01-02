import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LearningSuiteComponent } from './learning-suite.component';

describe('LearningSuiteComponent', () => {
  let component: LearningSuiteComponent;
  let fixture: ComponentFixture<LearningSuiteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LearningSuiteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LearningSuiteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
