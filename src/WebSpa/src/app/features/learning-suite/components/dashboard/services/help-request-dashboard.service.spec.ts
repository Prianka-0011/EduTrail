import { TestBed } from '@angular/core/testing';

import { HelpRequestDashboardService } from './help-request-dashboard.service';

describe('HelpRequestDashboardService', () => {
  let service: HelpRequestDashboardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HelpRequestDashboardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
