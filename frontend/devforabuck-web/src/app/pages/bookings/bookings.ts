import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { BookingList, BookingsService } from '../../service/bookings';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../shared/services/auth.service';

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
  notAllowed = false;

  private bookingsService = inject(BookingsService);
  public authService = inject(AuthService);
  private router = inject(Router);

  ngOnInit(): void {
    if (!this.authService.isLoggedIn) {
      this.notAllowed = true;
      this.loadBookings();
    }
  }

  loadBookings(): void {
    this.isLoading = true;

    this.bookingsService.getAllBookings().subscribe({
      next: (data) => {
        this.bookings = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load bookings:', err);
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
    const csvContent = this.convertToCSV(this.bookings);
    this.downloadFile(csvContent, 'bookings.csv');
  }

  cancelBooking(bookingId: string): void {
    // Implement cancel logic if needed
  }

  private convertToCSV(bookings: BookingList[]): string {
    const headers = ['Name', 'Email', 'Stack', 'Experience', 'Slot Time'];
    const rows = bookings.map(b => [
      `"${b.name}"`,
      `"${b.email}"`,
      `"${b.stack}"`,
      b.experienceYears,
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
