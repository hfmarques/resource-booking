using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Modules.Common.API;

public static class EndpointExtensions
{
    public static IServiceCollection RegisterApiEndpointsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var endpointTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsAssignableTo(typeof(IApiEndpoint)))
            .ToList();
        foreach (var type in endpointTypes) services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IApiEndpoint), type));

        return services;
    }

    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.ServiceProvider.GetRequiredService<IEnumerable<IApiEndpoint>>();
        foreach (var endpoint in endpoints) endpoint.MapEndpoint(app);

        return app;
    }
}
