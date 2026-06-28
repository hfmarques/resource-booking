using ModularMonolith.AppHost.Tests.Infrastructure;

namespace ModularMonolith.AppHost.Tests;

[Collection(DistributedApplicationCollection.Name)]
public class HealthEndpointTests(DistributedApplicationFixture fixture)
{
    [Fact]
    public async Task Health_Endpoint_Returns_Ok()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        using var httpClient = await fixture.CreateApiHttpClientAsync();

        // Act
        using var response = await httpClient.GetAsync("/health", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Alive_Endpoint_Returns_Ok()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        using var httpClient = await fixture.CreateApiHttpClientAsync();

        // Act
        using var response = await httpClient.GetAsync("/alive", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
