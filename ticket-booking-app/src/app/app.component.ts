import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  imports: [ReactiveFormsModule,HttpClientModule,CommonModule],  // Import ReactiveFormsModule
  standalone: true
})
export class AppComponent {
  bookingsForm: FormGroup;  // This will be our form group for the new booking

  bookings: any[] = [];  // This will hold the list of bookings

  constructor(private http: HttpClient, private fb: FormBuilder) {
    // Initialize the form using FormBuilder
    this.bookingsForm = this.fb.group({
      customerName: ['', Validators.required],  // Required field
      contactEmail: ['', [Validators.required, Validators.email]], // Required and email format
      contactPhoneNumber: ['', Validators.required],
      eventName: ['', Validators.required],
      numberOfTickets: [1, [Validators.required, Validators.min(1)]], // Minimum value of 1
      bookingDate: ['', Validators.required]
    });
  }

  // Create booking
  createBooking() {
    if (this.bookingsForm.invalid) {
      return;  // Don't proceed if the form is invalid
    }

    const newBooking = this.bookingsForm.value; // Get form values

    this.http.post('http://localhost:5240/api/bookings/CreateBooking', newBooking)
      .subscribe(
        response => {
          console.log('Booking created successfully!', response);
          this.loadBookings();  // Refresh the booking list
          this.clearForm();  // Clear form after submitting
        },
        error => {
          console.error('Error creating booking!', error);
        }
      );
  }

  // Load all bookings
  loadBookings() {
    this.http.get<any[]>('http://localhost:5000/api/bookings')
      .subscribe(
        data => {
          this.bookings = data;
        },
        error => {
          console.error('Error fetching bookings!', error);
        }
      );
  }

  // Delete booking
  deleteBooking(id: string) {
    this.http.delete(`http://localhost:5000/api/bookings/${id}`)
      .subscribe(
        response => {
          console.log('Booking deleted successfully!', response);
          this.loadBookings();  // Refresh the booking list
        },
        error => {
          console.error('Error deleting booking!', error);
        }
      );
  }

  // Clear form after booking submission
  clearForm() {
    this.bookingsForm.reset({
      numberOfTickets: 1 // reset to default value of 1
    });
  }

  // ngOnInit lifecycle hook to load bookings initially
  ngOnInit() {
    this.loadBookings();
  }
}
