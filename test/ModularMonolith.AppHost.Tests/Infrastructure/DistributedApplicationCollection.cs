namespace ModularMonolith.AppHost.Tests.Infrastructure;

[CollectionDefinition(Name)]
public sealed class DistributedApplicationCollection : ICollectionFixture<DistributedApplicationFixture>
{
    public const string Name = "DistributedApplication";
}
