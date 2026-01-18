using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Projekt.Data;
using Projekt.Endpoints;
using Projekt.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddSingleton<ProductSuggestionsService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapListsEndpoints();
app.MapItemsEndpoints();
app.MapSuggestionsEndpoints();

app.UseExceptionHandler(app =>
{
    app.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is ArgumentException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                error = exception.Message
            });
        }
    });
});


app.Run();