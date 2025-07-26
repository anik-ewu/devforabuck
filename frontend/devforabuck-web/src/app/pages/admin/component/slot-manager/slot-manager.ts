import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Slot, SlotService } from '../../../../service/slot-service';
// import { SlotService, Slot } from '../../services/slot.service';

@Component({
  selector: 'app-slot-manager',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './slot-manager.html',
  styleUrls: ['./slot-manager.scss'],
})
export class SlotManager implements OnInit {
  slotForm!: FormGroup;
  slots: Slot[] = [];
  allSlots: Slot[] = [];
  selectedType = '';
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
      error: (err: any) => console.error('Failed to load slots', err),
      complete: () => (this.loading = false),
    });
  }

  loadSlots(): void {
    this.loading = true;
    this.slotService.getAllSlots().subscribe({
      next: (data: Slot[]) => {
        this.allSlots = data;
        this.applyFilter();
      },
      error: (err: any) => console.error('Failed to load slots', err),
      complete: () => (this.loading = false),
    });
  }

  applyFilter(): void {
    this.slots = this.selectedType
      ? this.allSlots.filter((slot) => slot.slotType === this.selectedType)
      : [...this.allSlots];
  }

  resetFilters() {

  }
}
