// src/app/services/bookings.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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
  private baseUrl = 'https://devforabuck-api-dev-b8fga8a9gsezg4g8.southeastasia-01.azurewebsites.net/api';  // Replace with your API base URL

  constructor(private http: HttpClient) {}

  getAllBookings(): Observable<BookingList[]> {
    return this.http.get<BookingList[]>(`${this.baseUrl}/admin/all`);
  }
}
