using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.ResourceBooking.Core.Features.Shared.Routes;

namespace Modules.ResourceBooking.Core.Features.CreateReservation;

public sealed class CreateReservationEndpoint : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(RouteConsts.BaseRoute, async (
            CreateReservationRequest request,
            CreateReservationHandler handler,
            IValidator<CreateReservationRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.ToDictionary());
            }

            var result = await handler.HandleAsync(request);
            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            return Results.BadRequest(new { Error = result.Error });
        })
        .WithName("CreateReservation")
        .WithTags("Reservations");
    }
}
