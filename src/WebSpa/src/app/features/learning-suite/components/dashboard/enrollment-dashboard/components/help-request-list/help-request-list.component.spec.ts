import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HelpRequestListComponent } from './help-request-list.component';

describe('HelpRequestListComponent', () => {
  let component: HelpRequestListComponent;
  let fixture: ComponentFixture<HelpRequestListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HelpRequestListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HelpRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
