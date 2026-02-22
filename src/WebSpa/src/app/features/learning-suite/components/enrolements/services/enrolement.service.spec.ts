import { TestBed } from '@angular/core/testing';

import { EnrolementService } from './enrolement.service';

describe('EnrolementService', () => {
  let service: EnrolementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EnrolementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
