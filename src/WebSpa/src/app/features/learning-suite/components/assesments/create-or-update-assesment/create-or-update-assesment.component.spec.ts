import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateOrUpdateAssesmentComponent } from './create-or-update-assesment.component';

describe('CreateOrUpdateAssesmentComponent', () => {
  let component: CreateOrUpdateAssesmentComponent;
  let fixture: ComponentFixture<CreateOrUpdateAssesmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateOrUpdateAssesmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateOrUpdateAssesmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
