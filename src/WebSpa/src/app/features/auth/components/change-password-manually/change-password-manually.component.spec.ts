import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangePasswordManuallyComponent } from './change-password-manually.component';

describe('ChangePasswordManuallyComponent', () => {
  let component: ChangePasswordManuallyComponent;
  let fixture: ComponentFixture<ChangePasswordManuallyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChangePasswordManuallyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangePasswordManuallyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
