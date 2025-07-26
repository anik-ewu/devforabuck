import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Slot {
  id: string;
  startTime: string;
  endTime: string;
  slotType: string;
  isBooked: boolean;
  bookedByEmail?: string;
}

@Injectable({
  providedIn: 'root'
})
export class SlotService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAllSlots(): Observable<Slot[]> {
    return this.http.get<Slot[]>(`${this.apiUrl}/slots/queries/admin/allSlots`);
  }

  createSlot(payload: { startTime: string; endTime: string; slotType: string }): Observable<Slot> {
    return this.http.post<Slot>(`${this.apiUrl}/slots/commands`, payload);
  }
}

