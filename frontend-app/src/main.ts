import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { TokenInterceptorFn } from './app/interceptors/token.interceptor';
import { routes } from './app/app.routes'; // ✅ Import this

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(withInterceptors([TokenInterceptorFn])),
    provideRouter(routes)
  ]
});
