import { TestBed } from '@angular/core/testing';
import {
  HTTP_INTERCEPTORS,
  HttpClient
} from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { BookingsService } from './bookings-service';
import { AuthInterceptor } from '../shared/interceptors/auth.interceptor';
import { environment } from '../../environments/environment';

describe('BookingsService with AuthInterceptor', () => {
  let service: BookingsService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        BookingsService,
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
      ]
    });
    service = TestBed.inject(BookingsService);
    httpMock = TestBed.inject(HttpTestingController);
    localStorage.setItem('access_token', 'test-token');
  });

  afterEach(() => {
    localStorage.clear();
    httpMock.verify();
  });

  it('getAllBookings should include Authorization header', () => {
    service.getAllBookings().subscribe();
    const req = httpMock.expectOne(`${environment.apiUrl}/bookings/queries/admin/all`);
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush([]);
  });

  it('getBookingsByEmail should include Authorization header', () => {
    const email = 'test@example.com';
    service.getBookingsByEmail(email).subscribe();
    const req = httpMock.expectOne(`${environment.apiUrl}/bookings/queries/${email}`);
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush([]);
  });

  it('createBooking should include Authorization header', () => {
    const formData = new FormData();
    service.createBooking(formData).subscribe();
    const req = httpMock.expectOne(`${environment.apiUrl}/bookings/commands`);
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });
});
