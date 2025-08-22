import { Component, OnInit, inject } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BookingsService, BookingList } from '../../service/bookings-service';
import { BookingModalComponent } from '../components/modal/create-booking/create-booking';
// import { BookingModalComponent } from '../booking-modal/create-booking.component';

@Component({
  selector: 'app-bookings',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatProgressSpinnerModule
  ],
  providers: [DatePipe],
  templateUrl: './bookings.html',
  styleUrls: ['./bookings.scss'],
})
export class Bookings implements OnInit {
  bookings: BookingList[] = [];
  isLoading = true;
  error: string | null = null;
  notAllowed = false;

  displayedColumns = ['name', 'email', 'stack', 'experienceYears', 'slotTime', 'actions'];

  private bookingsService = inject(BookingsService);
  private dialog = inject(MatDialog);
  // Replace with real auth if needed
  private isLoggedIn = true;

  ngOnInit(): void {
    if (this.isLoggedIn) {
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

  exportBookings(): void {
    const csvContent = this.convertToCSV(this.bookings);
    this.downloadFile(csvContent, 'bookings.csv');
  }

  openBookingModal(): void {
    const dialogRef = this.dialog.open(BookingModalComponent, {
      width: '600px',
      maxWidth: '95vw',
      panelClass: 'booking-dialog-container',
      autoFocus: false,
      backdropClass: 'booking-backdrop'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (!result) return;

      // You said you'll call the API yourself — result.formData is ready
      console.log('Booking payload ready:', result);

      // Example: call your service here if/when you want
      // this.bookingsService.createBooking(result.formData).subscribe({...});

      // Optimistic UI (optional): reload list
      this.loadBookings();
    });
  }

  // utils
  private convertToCSV(bookings: BookingList[]): string {
    const headers = ['Name', 'Email', 'Stack', 'Experience', 'Slot Time'];
    const rows = bookings.map((b) => [
      `"${b.name}"`,
      `"${b.email}"`,
      `"${b.stack}"`,
      b.experienceYears,
      `"${new Date(b.slotTime).toLocaleString()}"`
    ]);
    return [headers.join(','), ...rows.map((r) => r.join(','))].join('\n');
  }

  private downloadFile(content: string, fileName: string): void {
    const blob = new Blob([content], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url; a.download = fileName; a.click();
    URL.revokeObjectURL(url);
  }
}
