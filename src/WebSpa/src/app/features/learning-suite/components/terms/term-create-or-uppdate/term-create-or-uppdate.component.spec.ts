import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TermCreateOrUppdateComponent } from './term-create-or-uppdate.component';

describe('TermCreateOrUppdateComponent', () => {
  let component: TermCreateOrUppdateComponent;
  let fixture: ComponentFixture<TermCreateOrUppdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TermCreateOrUppdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TermCreateOrUppdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
