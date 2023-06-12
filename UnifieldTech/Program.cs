using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UnifieldTech;
using UnifieldTech.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UnifieldTechContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UnifieldTechContext") ?? throw new InvalidOperationException("Connection string 'UnifieldTechContext' not found.")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapClienteEndpoints();

app.MapCelularEndpoints();

app.MapFazendaEndpoints();

app.Run();
