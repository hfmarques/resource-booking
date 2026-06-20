using Microsoft.EntityFrameworkCore;
using Modules.Common.Domain;
using Modules.ResourceBooking.Domain.Entities;
using Modules.ResourceBooking.Domain.Enums;
using Modules.ResourceBooking.Infrastructure.Persistence;

namespace Modules.ResourceBooking.Core.Features.CreateReservation;

public sealed class CreateReservationHandler(ResourceBookingDbContext dbContext) : IHandler
{
    public async Task<Result<CreateReservationResponse>> HandleAsync(CreateReservationRequest request)
    {
        var user = await dbContext.Users.FindAsync(request.UserId);
        if (user == null)
            return Result<CreateReservationResponse>.Failure("User not found.");

        if (!user.IsActive)
            return Result<CreateReservationResponse>.Failure("User is not active.");

        var resource = await dbContext.Resources.FindAsync(request.ResourceId);
        if (resource == null)
            return Result<CreateReservationResponse>.Failure("Resource not found.");

        if (!resource.IsAvailable)
            return Result<CreateReservationResponse>.Failure("Resource is not available.");

        if (request.StartTime <= DateTime.UtcNow)
            return Result<CreateReservationResponse>.Failure("Start time cannot be in the past.");

        if (request.EndTime <= request.StartTime)
            return Result<CreateReservationResponse>.Failure("End time must be after start time.");

        // Check for overlapping reservations
        var hasConflict = await dbContext.Reservations
            .AnyAsync(r =>
                r.ResourceId == request.ResourceId &&
                r.Status == ReservationStatus.Approved &&
                request.StartTime < r.EndTime &&
                request.EndTime > r.StartTime);

        if (hasConflict)
            return Result<CreateReservationResponse>.Failure("Resource is already reserved for this period.");

        var status = (request.EndTime - request.StartTime).TotalHours > 4 
            ? ReservationStatus.Pending 
            : ReservationStatus.Approved;

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ResourceId = request.ResourceId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = status
        };

        dbContext.Reservations.Add(reservation);
        await dbContext.SaveChangesAsync();

        return Result<CreateReservationResponse>.Success(new CreateReservationResponse(
            reservation.Id,
            status.ToString()
        ));
    }
}
