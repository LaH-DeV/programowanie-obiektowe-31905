using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Endpoints;
using Projekt.Seed;
using Projekt.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddSingleton<ProductSuggestionsService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    DbSeeder.Seed(scope.ServiceProvider);
}

app.MapListsEndpoints();
app.MapItemsEndpoints();
app.MapSuggestionsEndpoints();


app.Run();