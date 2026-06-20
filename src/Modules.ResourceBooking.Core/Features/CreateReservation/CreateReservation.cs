namespace Modules.ResourceBooking.Core.Features.CreateReservation;

public record CreateReservationRequest(
    Guid UserId,
    Guid ResourceId,
    DateTime StartTime,
    DateTime EndTime
);

public record CreateReservationResponse(
    Guid ReservationId,
    string Status
);
