import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { BookingsService } from '../../../../service/bookings-service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-booking-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,   // modal
    MatFormFieldModule, // form wrapper
    MatInputModule,    // <input matInput>
    MatSelectModule,   // <mat-select>
    MatButtonModule,   // buttons
    MatIconModule,
  ],
  templateUrl: './create-booking.html',
  styleUrls: ['./create-booking.scss']
})
export class BookingModalComponent {
  bookingForm: FormGroup;
  resumeFile: File | null = null;
  fileName: string | null = null;

  experienceOptions = [0, 1, 2, 3, '4+'];
  sessionTypes = ['Resume Review', 'Career Counselling'];

  // You can generate dynamic slots here
  slotTimes = [
    { label: 'Tomorrow 10:00 AM', value: new Date(new Date().setHours(10, 0, 0, 0)).toISOString() },
    { label: 'Tomorrow 2:00 PM', value: new Date(new Date().setHours(14, 0, 0, 0)).toISOString() },
    { label: 'Tomorrow 6:00 PM', value: new Date(new Date().setHours(18, 0, 0, 0)).toISOString() }
  ];

  constructor(
    private fb: FormBuilder,
    private bookingsService: BookingsService,
    public dialogRef: MatDialogRef<BookingModalComponent>
  ) {
    this.bookingForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      stack: ['', Validators.required],
      experienceYears: ['', Validators.required],
      slotTime: ['', Validators.required],
      sessionType: ['', Validators.required]
    });
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input?.files?.length) {
      this.resumeFile = input.files[0];
      this.fileName = this.resumeFile.name;
    }
  }

  submitForm() {
    if (this.bookingForm.valid) {
      const formData = new FormData();
      formData.append('Name', this.bookingForm.get('name')?.value);
      formData.append('Email', this.bookingForm.get('email')?.value);
      formData.append('Stack', this.bookingForm.get('stack')?.value);
      formData.append('ExperienceYears', this.bookingForm.get('experienceYears')?.value);
      formData.append('SlotTime', this.bookingForm.get('slotTime')?.value);
      formData.append('SessionType', this.bookingForm.get('sessionType')?.value);

      if (this.resumeFile) {
        formData.append('Resume', this.resumeFile, this.resumeFile.name);
      }

      this.bookingsService.createBooking(formData).subscribe({
        next: (res) => {
          console.log('Booking success:', res);
          this.dialogRef.close(res);
        },
        error: (err) => {
          console.error('Booking error:', err);
        }
      });
    } else {
      this.bookingForm.markAllAsTouched();
    }
  }

  cancel() {
    this.dialogRef.close();
  }
}
