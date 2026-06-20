using Microsoft.AspNetCore.Routing;

namespace Modules.ResourceBooking.Core;

public interface IApiEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
