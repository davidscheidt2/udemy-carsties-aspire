var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.CarstiesAspire_ApiService>("apiservice");

builder.AddProject<Projects.CarstiesAspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
