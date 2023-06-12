using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Twilio;
using UnifieldTech.Data;
using UnifieldTech.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UnifieldTechContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UnifieldTechContext") ?? throw new InvalidOperationException("Connection string 'UnifieldTechContext' not found.")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

TwilioClient.Init("AC2c556c8c4bbc592728c527807458b033", "15b5ff25237e8b7b2a8e1c4f205cacdb");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SMS API V1");
});

app.UseHttpsRedirection();

app.MapClienteEndpoints();
app.MapCelularEndpoints();
app.MapFazendaEndpoints();

//var smsEndpoints = new SmsEndpoints();
//smsEndpoints.Configure(app);


app.Run();
