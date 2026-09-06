# Conference Room Booking API

A RESTful API for managing conference room bookings built with ASP.NET Core 8 and SQLite.

## Business Context

The API allows businesses to manage their conference room inventory and handle client bookings. Rental costs are calculated automatically based on the time of day using a tiered pricing model.

## Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core 8 | Web API framework |
| Entity Framework Core 8 | ORM |
| SQLite | Database |
| Swagger | API documentation |

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [dotnet-ef CLI tool](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

### Run Locally

**1. Clone the repository**
```bash
git clone https://github.com/Monsteroooo/ABP_test.git
cd ABP_test
```

**2. Apply database migrations**
```bash
dotnet ef database update
```
This creates `conference.db` with seed data (3 rooms and 3 services).

**3. Start the server**
```bash
dotnet run
```

**4. Open Swagger UI**
```
http://localhost:<port>/swagger
```

## Seed Data

The database is pre-populated with the following data on first run:

**Rooms:**
| Name | Capacity | Base Rate (UAH/hr) |
|---|---|---|
| Зал A | 50 | 2000 |
| Зал B | 100 | 3500 |
| Зал C | 30 | 1500 |

**Services:**
| Name | Price (UAH) |
|---|---|
| Проєктор | 500 |
| Wi-Fi | 300 |
| Звук | 700 |

## Pricing Model

Rental cost is calculated per **30-minute slot**. Each slot is priced according to its start time:

| Time Range | Tariff | Multiplier |
|---|---|---|
| 06:00 – 09:00 | Morning | ×0.90 (-10%) |
| 09:00 – 12:00 | Standard | ×1.00 |
| **12:00 – 14:00** | **Peak** | **×1.15 (+15%)** |
| 14:00 – 18:00 | Standard | ×1.00 |
| 18:00 – 23:00 | Evening | ×0.80 (-20%) |
| 23:00 – 06:00 | — | Not allowed |

> Peak hours (12:00–14:00) take priority over Standard hours.

**Example:** Booking Зал A (2000 UAH/hr) from 10:00 to 14:00:
- 10:00–12:00 → 2 hrs × 2000 × 1.00 = **4000 UAH**
- 12:00–14:00 → 2 hrs × 2000 × 1.15 = **4600 UAH**
- **Total room cost: 8600 UAH**

## API Endpoints

### Rooms — `/api/rooms`

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/rooms` | Create a new conference room |
| `PATCH` | `/api/rooms/{id}` | Update room details (partial update) |
| `DELETE` | `/api/rooms/{id}` | Delete a room (blocked if bookings exist) |
| `GET` | `/api/rooms/available` | Search available rooms by date, time and capacity |

**Create Room — Request:**
```json
POST /api/rooms
{
  "name": "Зал D",
  "capacity": 75,
  "baseHourlyRate": 2500
}
```

**Search Available Rooms:**
```
GET /api/rooms/available?date=2024-09-01&startTime=10:00&endTime=14:00&capacity=50
```

---

### Bookings — `/api/bookings`

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/bookings` | Book a room with cost calculation |

**Request:**
```json
{
  "roomId": 1,
  "date": "2024-09-01",
  "startTime": "10:00",
  "durationHours": 4,
  "serviceIds": [1, 2]
}
```

**Response:**
```json
{
  "id": 1,
  "roomName": "Зал A",
  "startTime": "2024-09-01T10:00:00",
  "endTime": "2024-09-01T14:00:00",
  "durationHours": 4,
  "roomCost": 8600,
  "servicesCost": 800,
  "totalCost": 9400,
  "services": [
    { "name": "Проєктор", "price": 500 },
    { "name": "Wi-Fi", "price": 300 }
  ]
}
```

**Booking validation rules:**
- Bookings only allowed between **23:00 and 06:00**
- Duration must be a **multiple of 0.5** hours (e.g. 1, 1.5, 2)
- Returns `409 Conflict` if the room is already booked for the selected time

---

### Reports — `/api/reports`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/reports/revenue` | Revenue grouped by room for a date range |
| `GET` | `/api/reports/bookings` | List of bookings with optional filters |

**Revenue Report:**
```
GET /api/reports/revenue?dateFrom=2024-09-01&dateTo=2024-09-30
```

**Bookings List (all filters optional):**
```
GET /api/reports/bookings?roomId=1&dateFrom=2024-09-01&dateTo=2024-09-30
```

## Project Structure

```
ABP_test/
├── Controllers/
│   ├── RoomsController.cs       # Room management endpoints
│   ├── BookingsController.cs    # Booking endpoint
│   └── ReportsController.cs     # Analytics endpoints
├── Services/
│   ├── Interfaces/
│   │   ├── IRoomService.cs
│   │   ├── IBookingService.cs
│   │   ├── IPricingService.cs
│   │   └── IReportService.cs
│   ├── RoomService.cs
│   ├── BookingService.cs
│   ├── PricingService.cs        # Tariff calculation logic
│   └── ReportService.cs
├── Models/                      # EF Core entities
│   ├── Room.cs
│   ├── Service.cs
│   ├── Booking.cs
│   └── BookingService.cs        # Junction table
├── DTOs/
│   ├── Rooms/
│   ├── Bookings/
│   └── Reports/
├── Data/
│   └── AppDbContext.cs          # EF Core DbContext with seed data
├── Converters/
│   └── JsonConverters.cs        # DateOnly / TimeOnly JSON support
└── Migrations/
```

## HTTP Status Codes

| Code | Meaning |
|---|---|
| `200 OK` | Success |
| `201 Created` | Resource created |
| `204 No Content` | Deleted successfully |
| `400 Bad Request` | Invalid input data |
| `404 Not Found` | Resource not found |
| `409 Conflict` | Booking conflict or room has existing bookings |
