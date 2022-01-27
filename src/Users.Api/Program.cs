using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.ApplicationCore.Commands;
using Users.ApplicationCore.Interfaces;
using Users.ApplicationCore.Profiles;
using Users.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Initialize DB
var options = new DbContextOptionsBuilder<UsersDbContext>()
    .UseCosmos(
        builder.Configuration["Cosmos:ConnectionString"],
        builder.Configuration["Cosmos:DatabaseName"])
    .Options;

using (var context = new UsersDbContext(options))
{
    await context.Database.EnsureCreatedAsync();
}

builder.Services.AddControllers();

builder.Services.AddMediatR(typeof(CreateUserCommand).GetTypeInfo().Assembly);

builder.Services.AddAutoMapper(typeof(UserProfile).GetTypeInfo().Assembly);

builder.Services.AddDbContext<IUsersDbContext, UsersDbContext>(
      options => options.UseCosmos(
          builder.Configuration["Cosmos:ConnectionString"],
          builder.Configuration["Cosmos:DatabaseName"]));


builder.Services.AddHealthChecks()
    .AddCosmosDb(
        builder.Configuration["Cosmos:ConnectionString"],
        builder.Configuration["Cosmos:DatabaseName"]);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program { }
#pragma warning restore CA1050 // Declare types in namespaces
