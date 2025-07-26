import { CommonModule } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './admin.html',
  styleUrl: './admin.scss'
})
export class Admin {

  slotForm!: FormGroup;
  slots: any[] = [];
  selectedType: string = '';
  loading = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {}

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

    const body = {
      startTime: new Date(this.slotForm.value.startTime).toISOString(),
      endTime: new Date(this.slotForm.value.endTime).toISOString(),
      slotType: this.slotForm.value.slotType,
    };

    this.loading = true;

    this.http.post('/api/slots/commands', body).subscribe({
      next: () => {
        this.slotForm.reset({ slotType: 'resume' });
        this.loadSlots();
      },
      error: (err) => console.error('Failed to create slot', err),
      complete: () => (this.loading = false),
    });
  }

  loadSlots(): void {
    this.loading = true;

    let params = new HttpParams();
    if (this.selectedType) {
      params = params.set('type', this.selectedType);
    }

    this.http.get<any[]>('/api/slots/queries/admin/all', { params }).subscribe({
      next: (data) => (this.slots = data),
      error: (err) => console.error('Failed to load slots', err),
      complete: () => (this.loading = false),
    });
  }
}
