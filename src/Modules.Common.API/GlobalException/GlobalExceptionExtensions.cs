using Microsoft.Extensions.DependencyInjection;

namespace Modules.Common.API.GlobalException;

public static class GlobalExceptionExtensions
{
    public static IServiceCollection AddCommonApi(this IServiceCollection services)
    {
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }
}