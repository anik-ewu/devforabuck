import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookingList, BookingsService } from '../../service/bookings';
import { environment } from '../../../environments/environment';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-bookings',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './bookings.html',
  styleUrls: ['./bookings.scss']
})
export class Bookings implements OnInit {
  bookings: BookingList[] = [];
  isLoading = true;
  error: string | null = null;

  private bookingsService = inject(BookingsService);

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.isLoading = true;
    this.error = null;

    this.bookingsService.getAllBookings().subscribe({
      next: (data) => {
        this.bookings = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load bookings:', err);
        this.error = 'Failed to load bookings. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  refreshBookings(): void {
    console.log('Refreshing bookings...');
    this.loadBookings();
  }

  exportBookings(): void {
    console.log('Exporting bookings...');
    // Implement export logic here
    // This could be CSV, Excel, or PDF export
    const csvContent = this.convertToCSV(this.bookings);
    this.downloadFile(csvContent, 'bookings.csv');
  }

  cancelBooking(bookingId: string): void {
    // if (confirm('Are you sure you want to cancel this booking?')) {
    //   this.bookingsService.cancelBooking(bookingId).subscribe({
    //     next: () => {
    //       this.bookings = this.bookings.filter(b => b.id !== bookingId);
    //     },
    //     error: (err) => {
    //       console.error('Failed to cancel booking:', err);
    //       alert('Failed to cancel booking. Please try again.');
    //     }
    //   });
    // }
  }

  private convertToCSV(bookings: BookingList[]): string {
    const headers = ['Name', 'Email', 'Stack', 'Experience', 'Session Type', 'Slot Time'];
    const rows = bookings.map(b => [
      `"${b.name}"`,
      `"${b.email}"`,
      `"${b.stack}"`,
      b.experienceYears,
      // `"${b.sessionType || 'CV Review'}"`,
      `"${new Date(b.slotTime).toLocaleString()}"`
    ]);

    return [headers.join(','), ...rows.map(row => row.join(','))].join('\n');
  }

  private downloadFile(content: string, fileName: string): void {
    const blob = new Blob([content], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);

    link.setAttribute('href', url);
    link.setAttribute('download', fileName);
    link.style.visibility = 'hidden';

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }
}