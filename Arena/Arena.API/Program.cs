using Arena.API.Endpoints;
using Arena.Application.Services;
using Arena.Domain.Repositories;
using Arena.Infrastructure;
using Arena.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Configuration des services (DI) ---
builder.Services.AddDbContext<ArenaDbContext>(options =>
    options.UseSqlite("Data Source=arena.db"));

// --- Repositories : interface → implémentation concrète ---
// Pour basculer en InMemory, commenter les 2 lignes ci-dessous et décommenter les 2 suivantes.
builder.Services.AddScoped<ICreatureRepository, EfCreatureRepository>();
builder.Services.AddScoped<IBattleRepository, EfBattleRepository>();
// builder.Services.AddSingleton<ICreatureRepository, InMemoryCreatureRepository>();
// builder.Services.AddSingleton<IBattleRepository, InMemoryBattleRepository>();

// --- Services applicatifs ---
builder.Services.AddScoped<CreatureAppService>();
builder.Services.AddScoped<BattleAppService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Création automatique de la base de données
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ArenaDbContext>();
    db.Database.EnsureCreated();
}

// --- Swagger ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- Endpoints ---
app.MapCreatureEndpoints();
app.MapBattleEndpoints();

app.Run();
