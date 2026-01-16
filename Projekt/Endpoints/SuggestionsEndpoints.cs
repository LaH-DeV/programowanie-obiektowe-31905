using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;
using Projekt.DTOs;
using Projekt.Services;

namespace Projekt.Endpoints;

public static class SuggestionsEndpoints
{
    public static void MapSuggestionsEndpoints(this WebApplication app)
    {
        app.MapGet("/suggestions", (
            string? q,
            ProductSuggestionsService service) =>
        {
            if (string.IsNullOrWhiteSpace(q))
                return Results.Ok(Array.Empty<string>());

            var results = service.Suggestions
                .Where(p =>
                    p.StartsWith(q, StringComparison.OrdinalIgnoreCase) ||
                    p.Contains(q, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p)
                .Take(10)
                .ToList();

            return Results.Ok(results);
        });
    }
}
