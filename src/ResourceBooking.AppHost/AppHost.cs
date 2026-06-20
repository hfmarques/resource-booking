var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume()
    .AddDatabase("resourcebooking");

builder.AddProject<Projects.ModularMonolith_Host>("api")
    .WithReference(postgres)
    .WaitFor(postgres);

builder.Build().Run();