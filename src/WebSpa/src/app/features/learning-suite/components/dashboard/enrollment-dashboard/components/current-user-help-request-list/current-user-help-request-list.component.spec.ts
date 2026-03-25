import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentUserHelpRequestListComponent } from './current-user-help-request-list.component';

describe('CurrentUserHelpRequestListComponent', () => {
  let component: CurrentUserHelpRequestListComponent;
  let fixture: ComponentFixture<CurrentUserHelpRequestListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CurrentUserHelpRequestListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CurrentUserHelpRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
