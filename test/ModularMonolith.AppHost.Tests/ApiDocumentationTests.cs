using ModularMonolith.AppHost.Tests.Infrastructure;

namespace ModularMonolith.AppHost.Tests;

[Collection(DistributedApplicationCollection.Name)]
public class ApiDocumentationTests(DistributedApplicationFixture fixture)
{
    [Fact]
    public async Task OpenApi_Endpoint_Returns_Document()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        using var httpClient = await fixture.CreateApiHttpClientAsync();

        // Act
        using var response = await httpClient.GetAsync("/openapi/v1.json", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        var document = await response.Content.ReadAsStringAsync(cancellationToken);
        Assert.False(string.IsNullOrWhiteSpace(document));
        Assert.Contains("openapi", document);
    }

    [Fact]
    public async Task Scalar_Endpoint_Returns_Success()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        using var httpClient = await fixture.CreateApiHttpClientAsync();

        // Act
        using var response = await httpClient.GetAsync("/scalar", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Root_Redirects_To_Scalar_In_Development()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        using var httpClient = await fixture.CreateApiHttpClientAsync(followRedirects: false);

        // Act
        using var response = await httpClient.GetAsync("/", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/scalar", response.Headers.Location?.OriginalString);
    }
}
