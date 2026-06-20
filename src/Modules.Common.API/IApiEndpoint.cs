using Microsoft.AspNetCore.Routing;

namespace Modules.Common.API;

public interface IApiEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
