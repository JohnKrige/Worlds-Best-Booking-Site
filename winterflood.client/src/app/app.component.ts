import { HttpClient } from '@angular/common/http';
import { Component, computed, OnInit, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  bookings: any[] = [];
  inventory: any[] = [];

  selectedItemForBooking = signal<any>(null);
  selectedItemForEditing = signal<any>(null);

  startDate = signal<string>(this.today());
  endDate = signal<string>(this.tomorrow());

  totalDays = computed(() => {
    const start = this.toLocalDate(this.startDate());
    const end = this.toLocalDate(this.endDate());

    const diffMs = end.getTime() - start.getTime();

    return Math.floor(diffMs / (1000 * 60 * 60 * 24));
  })

  totalAmountSignal = computed(() => {
    if (this.selectedItemForBooking()) return this.selectedItemForBooking().pricePerDay * this.unitsToBook() * this.totalDays();

    return 0;
  })

  totalAmountEditSignal =
    computed(() => {
      if (this.selectedItemForEditing()) return this.selectedItemForEditing().pricePerUnit * this.unitsToBook() * this.totalDays();

      return 0;
    })

  unitsToBook = signal<number>(1);

  constructor(private http: HttpClient) {
  }

  ngOnInit() {
    this.getBookings();
    this.GetInventory();

    console.log("Now:", new Date());
    console.log("Locale:", new Date().toLocaleDateString('en-CA'));
  }

  getBookings() {
    this.http.get('https://localhost:7030/api/bookings').subscribe(
      (result: any) => {
        this.bookings = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  GetInventory() {
    this.http.get('https://localhost:7030/api/inventory').subscribe(
      (result: any) => {
        this.inventory = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  onViewItem(item: any) {
    this.selectedItemForEditing.set(null);
    this.selectedItemForBooking.set(item);
  }

  onCancelBooking() {
    this.selectedItemForBooking.set(null);
    this.selectedItemForEditing.set(null);
    this.resetValues();
 }

  onEditItem(item: any) {
    console.log(item);
    this.selectedItemForBooking.set(null);
    this.selectedItemForEditing.set(item);
    this.unitsToBook.set(item.numberOfItems);
    this.startDate.set(new Date(item.bookingStartDate).toLocaleDateString('en-CA'));
    this.endDate.set(new Date(item.bookingEndDate).toLocaleDateString('en-CA'));
  }

  onBook() {
    const requestBody: { inventoryId: any; numberOfItems: number; bookingStartDate?: string; bookingEndDate?: string } = {
      inventoryId: this.selectedItemForBooking().id,
      numberOfItems: this.unitsToBook()
    };

    if (!this.selectedItemForBooking().eventDate) {

      if (this.startDate > this.endDate) {
        console.log("start date is after end date")
        return;
      }

      requestBody.bookingStartDate = this.startDate();
      requestBody.bookingEndDate = this.endDate();
    };

    this.http.post('https://localhost:7030/api/bookings', requestBody).subscribe(
      () => {
        this.getBookings();
        this.resetValues();
      },
      (error) => {
        console.error(error);
      }
    );
  }

  onEdit() {
  const requestBody: { bookingId: number; numberOfItems: number; bookingStartDate ?: string; bookingEndDate ?: string
    } = {
        bookingId: this.selectedItemForEditing().id,
        numberOfItems: this.unitsToBook(),
    }

    if (!this.selectedItemForEditing().eventDate) {
      requestBody.bookingStartDate = this.startDate();
      requestBody.bookingEndDate = this.endDate();
    }

    this.http.patch('https://localhost:7030/api/bookings', requestBody).subscribe(
      () => {
        this.getBookings();
        this.resetValues();
      },
      (error) => {
        console.error(error);
      }
    );
  }

  onDelete() {
    this.http.delete(`https://localhost:7030/api/bookings/${this.selectedItemForEditing().id}`).subscribe(
      () => {
        this.getBookings();
        this.resetValues();
      },
      (error) => {
        console.error(error);
      }
    );
  }


  resetValues() {
    this.selectedItemForBooking.set(null);
    this.selectedItemForEditing.set(null);
    this.startDate.set(this.today());
    this.endDate.set(this.tomorrow());
    this.unitsToBook.set(1);
  }

  today(): string {
    const date = new Date();
    date.setDate(date.getDate() + 0);
    return date.toLocaleDateString('en-CA');
  }

  tomorrow() {
    const date = new Date(this.today());
    date.setDate(date.getDate() + 1);
    return date.toLocaleDateString('en-CA');
  }

  toLocalDate(dateString: string): Date {
  const [year, month, day] = dateString.split('-').map(Number);
  return new Date(year, month - 1, day);
}

  title = 'winterflood.client';
}
