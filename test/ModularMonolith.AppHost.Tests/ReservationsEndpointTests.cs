using System.Net.Http.Json;
using ModularMonolith.AppHost.Tests.Infrastructure;

namespace ModularMonolith.AppHost.Tests;

[Collection(DistributedApplicationCollection.Name)]
public class ReservationsEndpointTests(DistributedApplicationFixture fixture) : IAsyncLifetime
{
    private const string ReservationsRoute = "/api/reservations";

    public Task InitializeAsync() => fixture.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task CreateReservation_WithInvalidPayload_ReturnsBadRequest()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        using var httpClient = await fixture.CreateApiHttpClientAsync();

        var request = new
        {
            UserId = Guid.Empty,
            ResourceId = Guid.Empty,
            StartTime = DateTime.UtcNow.AddDays(-1),
            EndTime = DateTime.UtcNow.AddDays(-2)
        };

        // Act
        using var response = await httpClient.PostAsJsonAsync(ReservationsRoute, request, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        Assert.False(string.IsNullOrWhiteSpace(body));
    }

    [Fact]
    public async Task CreateReservation_WithUnknownUser_ReturnsUserNotFound()
    {
        var cancellationToken = CancellationToken.None;
        using var httpClient = await fixture.CreateApiHttpClientAsync();

        var request = new
        {
            UserId = Guid.NewGuid(),
            ResourceId = Guid.NewGuid(),
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(2)
        };

        // Act
        using var response = await httpClient.PostAsJsonAsync(ReservationsRoute, request, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        Assert.Contains("User not found.", body);
    }
}
