import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SlotService } from '../../service/slot-service';
// import { SlotService, Slot } from '../../services/slot.service';

export interface Slot {
  id: string;
  startTime: string;
  endTime: string;
  isBooked: boolean;
  bookedByEmail?: string;
  slotType: string;
}




@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './admin.html',
  styleUrl: './admin.scss',
})
export class Admin implements OnInit {
  slotForm!: FormGroup;
  slots: Slot[] = [];
  selectedType: string = '';
  loading = false;

  constructor(private fb: FormBuilder, private slotService: SlotService) {}

  ngOnInit(): void {
    this.slotForm = this.fb.group({
      startTime: ['', Validators.required],
      endTime: ['', Validators.required],
      slotType: ['resume', Validators.required],
    });

    this.loadSlots();
  }

  createSlot(): void {
    if (this.slotForm.invalid) return;

    const { startTime, endTime, slotType } = this.slotForm.value;

    const payload = {
      startTime: new Date(startTime).toISOString(),
      endTime: new Date(endTime).toISOString(),
      slotType,
    };

    this.loading = true;

    this.slotService.createSlot(payload).subscribe({
      next: () => {
        this.slotForm.reset({ slotType: 'resume' });
        this.loadSlots();
      },
      error: (err) => console.error('Failed to create slot', err),
      complete: () => (this.loading = false),
    });
  }

  allSlots: Slot[] = []; // keep raw list

  loadSlots(): void {
    this.loading = true;

    this.slotService.getAllSlots().subscribe({
      next: (data) => {
        this.allSlots = data;
        this.applyFilter();
      },
      error: (err) => console.error('Failed to load slots', err),
      complete: () => (this.loading = false),
    });
  }

  applyFilter(): void {
    if (this.selectedType) {
      this.slots = this.allSlots.filter(slot => slot.slotType === this.selectedType);
    } else {
      this.slots = [...this.allSlots];
    }
  }

}
