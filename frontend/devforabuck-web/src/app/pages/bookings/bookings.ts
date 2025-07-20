import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookingList, BookingsService } from '../../service/bookings';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
// import { AuthService } from '../../service/auth'; // ✅ your service

@Component({
  selector: 'app-bookings',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './bookings.html',
  styleUrls: ['./bookings.scss'],
})
export class Bookings implements OnInit {
  bookings: BookingList[] = [];
  isLoading = true;
  error: string | null = null;
  notAllowed = false;

  private bookingsService = inject(BookingsService);
  private auth = inject(AuthService);

  ngOnInit(): void {
    const isLoggedIn = this.auth.isLoggedIn;
    if (isLoggedIn) {
      this.loadBookings();
    } else {
      this.notAllowed = true;
      this.isLoading = false;
    }
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
      },
    });
  }

  refreshBookings(): void {
    this.loadBookings();
  }

  exportBookings(): void {
    const csvContent = this.convertToCSV(this.bookings);
    this.downloadFile(csvContent, 'bookings.csv');
  }

  private convertToCSV(bookings: BookingList[]): string {
    const headers = ['Name', 'Email', 'Stack', 'Experience', 'Slot Time'];
    const rows = bookings.map((b) => [
      `"${b.name}"`,
      `"${b.email}"`,
      `"${b.stack}"`,
      b.experienceYears,
      `"${new Date(b.slotTime).toLocaleString()}"`,
    ]);
    return [headers.join(','), ...rows.map((r) => r.join(','))].join('\n');
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
