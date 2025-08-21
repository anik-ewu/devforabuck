import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { BookingModalComponent } from '../components/modal/create-booking/create-booking';
// import { BookingModalComponent } from './booking-modal.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, MatDialogModule],
  templateUrl: './home.html',
  styleUrls: ['./home.scss']
})
export class Home {
  constructor(private dialog: MatDialog) {}

  openBookingModal() {
    const dialogRef = this.dialog.open(BookingModalComponent, {
      width: '600px',
      maxWidth: '95vw',
      panelClass: 'booking-dialog-container',
      autoFocus: false,
      backdropClass: 'booking-backdrop'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Booking confirmed:', result);
      } else {
        console.log('Booking cancelled');
      }
    });
  }

}
