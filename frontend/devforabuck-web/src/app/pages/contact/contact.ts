import { Component } from '@angular/core';
import { Footer } from '../../shared/footer/footer';
import { MatDialog } from '@angular/material/dialog';
import { BookingModalComponent } from '../components/modal/create-booking/create-booking';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [],
  templateUrl: './contact.html',
  styleUrl: './contact.scss'
})
export class Contact {

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
