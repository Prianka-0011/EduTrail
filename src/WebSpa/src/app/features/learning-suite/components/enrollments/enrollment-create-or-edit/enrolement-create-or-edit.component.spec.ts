import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnrolementCreateOrEditComponent } from './enrolement-create-or-edit.component';

describe('EnrolementCreateOrEditComponent', () => {
  let component: EnrolementCreateOrEditComponent;
  let fixture: ComponentFixture<EnrolementCreateOrEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EnrolementCreateOrEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EnrolementCreateOrEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
