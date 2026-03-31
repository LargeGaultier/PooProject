using Arena.API.Endpoints;
using Arena.Business;
using Arena.DataAccess;
using Arena.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Configuration des services (DI) ---
builder.Services.AddDbContext<ArenaDbContext>(options =>
    options.UseSqlite("Data Source=arena.db"));

builder.Services.AddScoped<CreatureRepository>();
builder.Services.AddScoped<BattleRepository>();
builder.Services.AddScoped<CreatureService>();
builder.Services.AddScoped<BattleService>();

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
