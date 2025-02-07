﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Esport.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization; // Zakładając, że EsportDbContext znajduje się tutaj

var builder = WebApplication.CreateBuilder(args);

// Rejestracja DbContext (przykładowa konfiguracja)
builder.Services.AddDbContext<EsportDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ignoruj cykle – serializer nie będzie próbował serializować właściwości powodujących cykle
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // Możesz również ustawić opcjonalnie inne opcje, np. zachowanie formatowania
        // options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientApp", policy =>
    {
        // Upewnij się, że wpisujesz właściwy adres aplikacji klienta
        policy.WithOrigins("https://localhost:7068")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Dalsza konfiguracja...
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowClientApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
