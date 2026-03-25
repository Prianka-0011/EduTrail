import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HelpRequestDetailViewComponent } from './help-request-detail-view.component';

describe('HelpRequestDetailViewComponent', () => {
  let component: HelpRequestDetailViewComponent;
  let fixture: ComponentFixture<HelpRequestDetailViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HelpRequestDetailViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HelpRequestDetailViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
