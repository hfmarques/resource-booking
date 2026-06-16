# Resource Booking - Specification

## 1. Overview

Resource Booking is a small-to-medium .NET study project designed to test a new application architecture pattern.

The system allows users to reserve shared resources such as meeting rooms, projectors, laptops, or vehicles. It includes basic CRUD operations, business rules, domain validations, and clear use cases suitable for testing Clean Architecture, Vertical Slice Architecture, CQRS, DDD, or Modular Monolith approaches.

---

## 2. Goals

The main goals of this project are:

* Test a new architecture pattern in .NET.
* Practice domain modeling.
* Separate business rules from infrastructure concerns.
* Implement clear application use cases.
* Support automated testing.
* Keep the project small enough for study, but complex enough to validate architectural decisions.

---

## 3. Domain

### 3.1 Main Entities

#### User

Represents a person who can reserve resources.

Properties:

* Id
* FirstName
* LastName
* Email
* IsActive

Rules:

* A user must have a valid email.
* Only active users can create reservations.

---

#### Resource

Represents something that can be reserved.

Examples:

* Meeting room
* Projector
* Laptop
* Vehicle

Properties:

* Id
* Name
* Description
* Type
* IsAvailable

Rules:

* A resource must have a name.
* Disabled resources cannot be reserved.

---

#### Reservation

Represents a booking made by a user for a resource.

Properties:

* Id
* UserId
* ResourceId
* StartTime
* EndTime
* Status

Possible statuses:

* Pending
* Approved
* Rejected
* Cancelled

Rules:

* Start time must be before end time.
* A reservation cannot be created in the past.
* A resource cannot have overlapping approved reservations.
* Reservations longer than 4 hours require approval.
* Reservations up to 4 hours may be automatically approved.
* A reservation can only be cancelled before it starts.

---

## 4. Use Cases

### 4.1 Create Reservation

Allows an active user to create a reservation for an available resource.

Input:

* UserId
* ResourceId
* StartTime
* EndTime

Rules:

* User must be active.
* Resource must be available.
* Period must be valid.
* Reservation must not conflict with another approved reservation.
* If duration is greater than 4 hours, status should be Pending.
* Otherwise, status should be Approved.

Output:

* ReservationId
* Status

---

### 4.2 Approve Reservation

Approves a pending reservation.

Input:

* ReservationId

Rules:

* Reservation must exist.
* Reservation must be Pending.
* Reservation must not conflict with another approved reservation.

Output:

* Updated reservation status.

---

### 4.3 Reject Reservation

Rejects a pending reservation.

Input:

* ReservationId

Rules:

* Reservation must exist.
* Reservation must be Pending.

Output:

* Updated reservation status.

---

### 4.4 Cancel Reservation

Cancels an existing reservation.

Input:

* ReservationId

Rules:

* Reservation must exist.
* Reservation cannot already be Cancelled.
* Reservation cannot be cancelled after it has started.

Output:

* Updated reservation status.

---

### 4.5 Get Available Resources

Lists resources available for a specific time period.

Input:

* StartTime
* EndTime
* Optional ResourceType

Rules:

* Period must be valid.
* Disabled resources should not be returned.
* Resources with conflicting approved reservations should not be returned.

Output:

* List of available resources.

---

## 5. Suggested Architecture

The project can be organized using Clean Architecture:

```text
src/
  ResourceBooking.Api/
  ResourceBooking.Application/
  ResourceBooking.Domain/
  ResourceBooking.Infrastructure/

tests/
  ResourceBooking.UnitTests/
  ResourceBooking.IntegrationTests/
```

---

## 6. Project Layers

### 6.1 Domain Layer

Contains enterprise business rules.

Responsibilities:

* Entities
* Enums
* Value Objects
* Domain Events
* Domain Services
* Domain Exceptions

Should not depend on:

* Entity Framework
* ASP.NET Core
* External APIs
* Database providers

---

### 6.2 Application Layer

Contains use cases.

Responsibilities:

* Commands
* Queries
* Handlers
* DTOs
* Interfaces
* Application validations

Examples:

```text
CreateReservationCommand
ApproveReservationCommand
CancelReservationCommand
GetAvailableResourcesQuery
```

---

### 6.3 Infrastructure Layer

Contains external implementations.

Responsibilities:

* Database access
* Entity Framework Core configuration
* Email service implementation
* Date/time provider implementation

---

### 6.4 API Layer

Exposes the system through HTTP endpoints.

Responsibilities:

* Minimal APIs
* Request/response mapping
* Authentication
* Swagger
* Dependency injection setup

---

## 7. API Endpoints

### Users

```http
POST /users
GET /users/{id}
PATCH /users/{id}/deactivate
```

### Resources

```http
POST /resources
GET /resources
GET /resources/{id}
PATCH /resources/{id}/enable
PATCH /resources/{id}/disable
```

### Reservations

```http
POST /reservations
GET /reservations/{id}
GET /reservations/user/{userId}
PATCH /reservations/{id}/approve
PATCH /reservations/{id}/reject
PATCH /reservations/{id}/cancel
```

### Availability

```http
GET /availability?startTime={startTime}&endTime={endTime}&resourceType={resourceType}
```

---

## 8. Database Model

Suggested tables:

```text
Users
Resources
Reservations
```

### Users

```text
Id
FirstName
LastName
Email
IsActive
CreatedAt
UpdatedAt
```

### Resources

```text
Id
Name
Description
Type
IsAvailable
CreatedAt
UpdatedAt
```

### Reservations

```text
Id
UserId
ResourceId
StartTime
EndTime
Status
CreatedAt
UpdatedAt
```

---

## 9. Business Rules Summary

| Rule   | Description                                               |
| ------ | --------------------------------------------------------- |
| BR-001 | A reservation start time must be before the end time.     |
| BR-002 | A reservation cannot be created in the past.              |
| BR-003 | A disabled resource cannot be reserved.                   |
| BR-004 | An inactive user cannot create reservations.              |
| BR-005 | A resource cannot have overlapping approved reservations. |
| BR-006 | Reservations longer than 4 hours require approval.        |
| BR-007 | Reservations up to 4 hours may be automatically approved. |
| BR-008 | A reservation cannot be cancelled after it starts.        |

---

## 10. Testing Strategy

### Unit Tests

Should cover:

* Entity behavior
* Value Object validation
* Domain services
* Business rules
* Command handlers

Examples:

```text
Should_Not_Create_Reservation_When_StartTime_Is_After_EndTime
Should_Not_Approve_Reservation_When_There_Is_Conflict
Should_Create_Pending_Reservation_When_Duration_Is_Greater_Than_4_Hours
Should_Create_Approved_Reservation_When_Duration_Is_Up_To_4_Hours
```

---

### Integration Tests

Should cover:

* API endpoints
* Database persistence
* Repository behavior
* Full use case execution

Examples:

```text
POST /reservations should create a reservation
PATCH /reservations/{id}/approve should approve a pending reservation
GET /availability should return only available resources
```

---

## 11. Optional Features

These features can be added later if needed:

* JWT authentication
* Role-based authorization
* Email notifications
* Background job to expire pending reservations
* Audit log
* Docker Compose
* PostgreSQL or SQL Server
* OpenTelemetry
* Redis cache
* Outbox pattern
* Domain events
* CQRS with MediatR

---

## 12. Definition of Done

The project is considered complete when:

* Users can be created and deactivated.
* Resources can be created, enabled, and disabled.
* Reservations can be created, approved, rejected, and cancelled.
* Availability can be queried.
* Main business rules are covered by tests.
* The architecture keeps domain logic independent from infrastructure.
* The project can run locally with database migrations.
* Swagger documentation is available.
