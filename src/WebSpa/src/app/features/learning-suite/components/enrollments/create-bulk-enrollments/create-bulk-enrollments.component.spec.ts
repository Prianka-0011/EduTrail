import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBulkEnrollmentsComponent } from './create-bulk-enrollments.component';

describe('CreateBulkEnrollmentsComponent', () => {
  let component: CreateBulkEnrollmentsComponent;
  let fixture: ComponentFixture<CreateBulkEnrollmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateBulkEnrollmentsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateBulkEnrollmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
