using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMediatR(typeof(Program).GetTypeInfo().Assembly);

builder.Services.AddAutoMapper(typeof(Program).GetTypeInfo().Assembly);

builder.Services.AddDbContext<UsersDbContext>(
      options => options.UseCosmos(
          builder.Configuration["Cosmos:ConnectionString"],
          builder.Configuration["Cosmos:DatabaseName"]));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
