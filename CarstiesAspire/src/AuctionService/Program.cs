using AuctionService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

// Add services to the container.

builder.Services.AddControllers();
builder.AddNpgsqlDbContext<AuctionDbContext>("auctions", configureDbContextOptions: options =>
{
    options.UseNpgsql();
});




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
    await EnsureDatabaseAsync(db);
    await db.Database.MigrateAsync();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task EnsureDatabaseAsync(AuctionDbContext dbContext, CancellationToken cancellationToken = default)
{
    var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

    var strategy = dbContext.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
        // Create the database if it does not exist.
        // Do this first so there is then a database to start a transaction against.
        if (!await dbCreator.ExistsAsync(cancellationToken))
        {
            await dbCreator.CreateAsync(cancellationToken);
        }
    });
}