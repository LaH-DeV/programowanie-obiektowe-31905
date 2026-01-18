using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;
using Projekt.DTOs;

namespace Projekt.Endpoints;

public static class ItemsEndpoints {
  public static void MapItemsEndpoints(this WebApplication app) {

    app.MapPut("/items/{id}", async (int id, UpdateShoppingItemDto dto, AppDbContext db) => {
      var item = await db.Items.FindAsync(id);
      if (item == null) return Results.NotFound();

      item.Update(dto.Name.Trim(), dto.Quantity);

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

      item.MarkAsUnbought();
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

      item.MarkAsBought();
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

    app.MapGet("/items", async (int id, AppDbContext db) => {
      var item = await db.Items.FindAsync(id);
      if (item == null) return Results.NotFound();

      db.Items.Remove(item);
      await db.SaveChangesAsync();
      return Results.NoContent();
    });
  }
}