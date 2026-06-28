using System.Data.Common;
using Aspire.Hosting;
using Modules.ResourceBooking.Infrastructure.Persistence;
using Npgsql;
using Respawn;

namespace ModularMonolith.AppHost.Tests.Infrastructure;

public sealed class DistributedApplicationFixture : IAsyncLifetime
{
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    public DistributedApplication App { get; private set; } = null!;

    private DbConnection _dbConnection = null!;

    private Respawner _respawner = null!;

    public async Task InitializeAsync()
    {
        var cancellationToken = CancellationToken.None;

        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.ModularMonolith_AppHost>(cancellationToken);

        App = await appHost.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        await App.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);

        await EnsureDatabaseSchemaCreatedAsync(cancellationToken);
    }

    private async Task EnsureDatabaseSchemaCreatedAsync(CancellationToken cancellationToken)
    {
        await App.ResourceNotifications
            .WaitForResourceHealthyAsync("resourcebooking", cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);

        var connectionString = await App.GetConnectionStringAsync("resourcebooking", cancellationToken);

        var options = new DbContextOptionsBuilder<ResourceBookingDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var dbContext = new ResourceBookingDbContext(options);
        await dbContext.Database.EnsureCreatedAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);

        await InitializeRespawnerAsync(connectionString);
    }

    private async Task InitializeRespawnerAsync(string? connectionString)
    {
        _dbConnection = new NpgsqlConnection(connectionString);
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task<HttpClient> CreateApiHttpClientAsync(bool followRedirects = true)
    {
        var cancellationToken = CancellationToken.None;

        var defaultClient = App.CreateHttpClient("api", "http");

        await App.ResourceNotifications
            .WaitForResourceHealthyAsync("api", cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);

        if (followRedirects)
        {
            return defaultClient;
        }

        var baseAddress = defaultClient.BaseAddress;
        defaultClient.Dispose();

        return new HttpClient(new HttpClientHandler { AllowAutoRedirect = false })
        {
            BaseAddress = baseAddress
        };
    }

    public async Task DisposeAsync()
    {
        if (_dbConnection is not null)
        {
            await _dbConnection.DisposeAsync();
        }

        if (App is not null)
        {
            await App.DisposeAsync();
        }
    }
}
