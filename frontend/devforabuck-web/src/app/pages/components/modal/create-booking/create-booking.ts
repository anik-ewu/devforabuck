import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

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
    MatButtonModule    // buttons
  ],
  templateUrl: './create-booking.html',
  styleUrls: ['./create-booking.scss']
})
export class BookingModalComponent {
  bookingForm: FormGroup;

  experienceOptions = [0, 1, 2, 3, '4+'];
  sessionTypes = ['Resume Review', 'Career Counselling'];
  slotTimes = [
    { label: 'Tomorrow 10:00 AM', value: new Date(new Date().setHours(10, 0, 0)) },
    { label: 'Tomorrow 2:00 PM', value: new Date(new Date().setHours(14, 0, 0)) },
    { label: 'Tomorrow 6:00 PM', value: new Date(new Date().setHours(18, 0, 0)) }
  ];

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<BookingModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.bookingForm = this.fb.group({
      name: ['aa', Validators.required],
      email: ['a@gmail.com', [Validators.required, Validators.email]],
      stack: ['a', Validators.required],
      experienceYears: ['0', Validators.required],
      slotTime: ['', Validators.required],
      sessionType: ['', Validators.required],
      resume: [null, Validators.required]
    });
  }

  // Add this property to your component class
  fileName: string = '';

  // Update the onFileChange method
  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.fileName = file.name;
      // Your existing file handling logic
    }
  }

  submitForm() {
    if (this.bookingForm.valid) {
      console.log('Booking Data:', this.bookingForm.value);
      this.dialogRef.close(this.bookingForm.value);
    } else {
      console.log("hi", this.bookingForm.value);
      this.bookingForm.markAllAsTouched();
    }
  }

  cancel() {
    this.dialogRef.close();
  }
}
