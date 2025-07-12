import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface BookingList {
  id: string;
  name: string;
  email: string;
  stack: string;
  experienceYears: number;
  slotTime: string;
}

@Injectable({
  providedIn: 'root'
})
export class BookingsService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAllBookings(): Observable<BookingList[]> {
    return this.http.get<BookingList[]>(`${this.apiUrl}/bookings/admin/all`);
  }

  getBookingsByEmail(email: string): Observable<BookingList[]> {
    return this.http.get<BookingList[]>(`${this.apiUrl}/bookings/${email}`);
  }
}