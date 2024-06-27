var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres").WithPgAdmin();

var db = postgres.AddDatabase("auctions");

builder.AddProject<Projects.AuctionService>("auctionservice")
    .WithReference(db);

builder.Build().Run();