import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SlotManager } from './component/slot-manager/slot-manager';
import { BookingManager } from './component/booking-manager/booking-manager';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, SlotManager, BookingManager],
  templateUrl: './admin.html',
  styleUrl: './admin.scss',
})
export class Admin {
  activeTab: 'slots' | 'bookings' = 'slots';
}
