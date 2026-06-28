using ModularMonolith.AppHost.Tests.Infrastructure;

namespace ModularMonolith.AppHost.Tests;

[Collection(DistributedApplicationCollection.Name)]
public class AppHostResourceTests(DistributedApplicationFixture fixture)
{
    [Fact]
    public async Task Postgres_Resource_Reaches_Healthy_State()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act / Assert
        await fixture.App.ResourceNotifications
            .WaitForResourceHealthyAsync("postgres", cancellationToken)
            .WaitAsync(DistributedApplicationFixture.DefaultTimeout, cancellationToken);
    }

    [Fact]
    public async Task Api_Resource_Reaches_Healthy_State()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act / Assert
        await fixture.App.ResourceNotifications
            .WaitForResourceHealthyAsync("api", cancellationToken)
            .WaitAsync(DistributedApplicationFixture.DefaultTimeout, cancellationToken);
    }
}
