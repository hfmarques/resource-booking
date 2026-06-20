using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Modules.Common.Domain;

namespace Modules.Common.API;

public static class HandlerExtensions
{
    public static IServiceCollection RegisterHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsAssignableTo(typeof(IHandler)))
            .ToList();

        foreach (var type in handlerTypes)
        {
            services.AddTransient(type);
        }

        return services;
    }

    public static IServiceCollection RegisterHandlersFromAssemblyContaining<T>(this IServiceCollection services)
        => services.RegisterHandlersFromAssembly(typeof(T).Assembly);
}
