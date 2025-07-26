import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookingList, BookingsService } from '../../../../service/bookings-service';
// import { BookingsService, BookingList } from '../../services/bookings.service';

@Component({
  selector: 'app-booking-manager',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './booking-manager.html',
  styleUrls: ['./booking-manager.scss'],
})
export class BookingManager implements OnInit {
  allBookings: BookingList[] = [];
  filteredBookings: BookingList[] = [];
  bookingEmailFilter = '';

  constructor(private bookingsService: BookingsService) {}

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.bookingsService.getAllBookings().subscribe({
      next: (bookings) => {
        this.allBookings = bookings;
        this.applyBookingFilter();
      },
      error: (err) => console.error('Booking load failed', err),
    });
  }

  applyBookingFilter(): void {
    const email = this.bookingEmailFilter.toLowerCase();
    this.filteredBookings = this.allBookings.filter(b =>
      b.email.toLowerCase().includes(email)
    );
  }
}
