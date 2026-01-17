using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace Projekt.Services;

public class ProductSuggestionsService
{
    public IReadOnlyList<string> Suggestions { get; }

    public ProductSuggestionsService(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "products.txt");

        Suggestions = File.Exists(path)
            ? File.ReadAllLines(path)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList()
            : [];
    }
}
