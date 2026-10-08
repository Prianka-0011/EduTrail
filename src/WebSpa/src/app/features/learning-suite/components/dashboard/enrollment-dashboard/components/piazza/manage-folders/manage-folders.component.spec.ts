import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageFoldersComponent } from './manage-folders.component';

describe('ManageFoldersComponent', () => {
  let component: ManageFoldersComponent;
  let fixture: ComponentFixture<ManageFoldersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageFoldersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageFoldersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
