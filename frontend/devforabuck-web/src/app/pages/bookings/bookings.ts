import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookingList, BookingsService } from '../../service/bookings';
import { environment } from '../../../environments/environment';
// import { environment } from '../../environments/environment';

@Component({
  selector: 'app-bookings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bookings.html',
  styleUrl: './bookings.scss'
})
export class Bookings implements OnInit {
  bookings: BookingList[] = [];
  private bookingsService = inject(BookingsService);

  ngOnInit() {
    console.log("Fetching bookings from:", environment.apiUrl);
    this.bookingsService.getAllBookings().subscribe({
      next: (data) => {
        console.log('Bookings:', data);
        this.bookings = data;
      },
      error: (err) => {
        console.error('Failed to load bookings:', err);
      }
    });
  }
}