using FluentValidation;
using Modules.ResourceBooking.Core.Features.CreateReservation;

namespace Modules.ResourceBooking.Core.Features.CreateReservation;

public class CreateReservationValidator : AbstractValidator<CreateReservationRequest>
{
    public CreateReservationValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ResourceId).NotEmpty();
        RuleFor(x => x.StartTime).NotEmpty().GreaterThan(DateTime.UtcNow);
        RuleFor(x => x.EndTime).NotEmpty().GreaterThan(x => x.StartTime);
    }
}
