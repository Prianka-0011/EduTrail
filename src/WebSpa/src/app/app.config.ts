import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { routes } from './app.routes';
import { CredentialInterceptor } from './interceptors/auth.interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideNzIcons } from 'ng-zorro-antd/icon';
import { FolderOutline, FileOutline } from '@ant-design/icons-angular/icons';

export const appConfig: ApplicationConfig = {
  providers: [
    provideNzIcons([
      FolderOutline,
      FileOutline
    ]),
    provideAnimationsAsync(),
    provideZoneChangeDetection({ eventCoalescing: true }),

    provideRouter(routes, withComponentInputBinding()),

    provideClientHydration(withEventReplay()),

    provideHttpClient(withInterceptorsFromDi()),

    importProvidersFrom(
      BrowserAnimationsModule, // must come first
      ToastrModule.forRoot({
        positionClass: 'toast-bottom-right',
        preventDuplicates: true
      }),

    ),
    { provide: HTTP_INTERCEPTORS, useClass: CredentialInterceptor, multi: true }
  ]
};
