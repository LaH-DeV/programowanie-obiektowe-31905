using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;
using Projekt.DTOs;

namespace Projekt.Endpoints;

public static class ItemsEndpoints {
  public static void MapItemsEndpoints(this WebApplication app) {

    app.MapGet("/items", async (AppDbContext db) => await db.Items.ToListAsync());
    app.MapPost("/items", async (CreateShoppingItemDto dto, AppDbContext db) => {
      if (string.IsNullOrWhiteSpace(dto.Name))
        return Results.BadRequest("Name is required");

      if (dto.Quantity <= 0)
        return Results.BadRequest("Quantity must be greater than 0");

      var item = new ShoppingItem {
        Name = dto.Name.Trim(),
          Quantity = dto.Quantity,
          IsBought = false
      };

      db.Items.Add(item);
      await db.SaveChangesAsync();

      return Results.Created($"/items/{item.Id}", new {
        item.Id
      });
    });

    app.MapPut("/items/{id}", async (int id, CreateShoppingItemDto dto, AppDbContext db) => {
      var item = await db.Items.FindAsync(id);
      if (item == null) return Results.NotFound();

      if (string.IsNullOrWhiteSpace(dto.Name))
        return Results.BadRequest("Name is required");

      if (dto.Quantity <= 0)
        return Results.BadRequest("Quantity must be greater than 0");

      item.Name = dto.Name.Trim();
      item.Quantity = dto.Quantity;

      await db.SaveChangesAsync();
      return Results.Ok(new {
        item.Id,
          item.Name,
          item.Quantity,
          item.IsBought,
          item.ShoppingListId
      });
    });

    app.MapPut("/items/{id}/unmark", async (int id, AppDbContext db) => {
      var item = await db.Items.FindAsync(id);
      if (item == null) return Results.NotFound();

      item.IsBought = false;
      await db.SaveChangesAsync();
      return Results.Ok(new {
        item.Id,
          item.Name,
          item.Quantity,
          item.IsBought,
          item.ShoppingListId
      });
    });

    app.MapPut("/items/{id}/mark", async (int id, AppDbContext db) => {
      var item = await db.Items.FindAsync(id);
      if (item == null) return Results.NotFound();

      item.IsBought = true;
      await db.SaveChangesAsync();
      return Results.Ok(new {
        item.Id,
          item.Name,
          item.Quantity,
          item.IsBought,
          item.ShoppingListId
      });
    });

    app.MapDelete("/items/{id}", async (int id, AppDbContext db) => {
      var item = await db.Items.FindAsync(id);
      if (item == null) return Results.NotFound();

      db.Items.Remove(item);
      await db.SaveChangesAsync();
      return Results.NoContent();
    });
  }
}