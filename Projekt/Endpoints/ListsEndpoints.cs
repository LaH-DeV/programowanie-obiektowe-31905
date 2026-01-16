using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;
using Projekt.DTOs;

namespace Projekt.Endpoints;

public static class ListsEndpoints {
  public static void MapListsEndpoints(this WebApplication app) {
    app.MapGet(
      "/lists",
      async (AppDbContext db) =>
        await db.Lists
        .Select(l => new {
          l.Id, l.Name,
            ItemCount = db.Items.Count(i => i.ShoppingListId == l.Id),
            BoughtItemCount = db.Items.Count(
              i => i.ShoppingListId == l.Id && i.IsBought)
        })
        .ToListAsync());
    app.MapGet(
      "/lists/{listId}",
      async (int listId, AppDbContext db) =>
        await db.Lists
        .Select(l => new {
          l.Id, l.Name,
            Items =
            db.Items.Select(i => new {
          i.Id, i.Name, i.Quantity, i.IsBought,
            i.ShoppingListId
        }).Where(i => i.ShoppingListId == l.Id).ToList()
        })
        .FirstOrDefaultAsync(l => l.Id == listId) is {}
      list ?
      Results.Ok(list) :
      Results.NotFound());
    app.MapPost("/lists",
      async (CreateShoppingListDto dto, AppDbContext db) => {
        if (string.IsNullOrWhiteSpace(dto.Name))
          return Results.BadRequest("Name is required");

        var list = new ShoppingList {
          Name = dto.Name.Trim()
        };

        db.Lists.Add(list);
        await db.SaveChangesAsync();

        return Results.Created($"/lists/{list.Id}", new {
          list.Id
        });
      });

    app.MapPut("/lists/{listId}", async (int listId, CreateShoppingListDto dto,
      AppDbContext db) => {
      var list = await db.Lists.FindAsync(listId);
      if (list == null)
        return Results.NotFound();

      if (string.IsNullOrWhiteSpace(dto.Name))
        return Results.BadRequest("Name is required");

      list.Name = dto.Name.Trim();
      await db.SaveChangesAsync();
      return Results.Ok(new {
        list.Id, list.Name
      });
    });

    app.MapGet("/lists/{listId}/items",
      async (int listId, AppDbContext db) =>
        await db.Items
        .Select(i => new {
          i.Id, i.Name, i.Quantity, i.IsBought,
            i.ShoppingListId
        })
        .Where(i => i.ShoppingListId == listId)
        .ToListAsync());

    app.MapPost(
      "/lists/{listId}/items",
      async (int listId, CreateShoppingItemDto dto, AppDbContext db) => {
        if (string.IsNullOrWhiteSpace(dto.Name))
          return Results.BadRequest("Name is required");

        if (dto.Quantity <= 0)
          return Results.BadRequest("Quantity must be > 0");

        var listExists = await db.Lists.AnyAsync(l => l.Id == listId);
        if (!listExists)
          return Results.NotFound("List not found");

        var item = new ShoppingItem {
          Name = dto.Name.Trim(),
            Quantity = dto.Quantity,
            ShoppingListId = listId
        };

        db.Items.Add(item);
        await db.SaveChangesAsync();

        return Results.Created($"/items/{item.Id}", new {
          item.Id
        });
      });

    app.MapDelete("/lists/{listId}", async (int listId, AppDbContext db) => {
      var list = await db.Lists.FindAsync(listId);
      if (list == null)
        return Results.NotFound();
      // cascade delete items
      // could it be a transaction?
      db.Lists.Remove(list);
      db.Items.RemoveRange(db.Items.Where(i => i.ShoppingListId == listId));
      await db.SaveChangesAsync();

      return Results.NoContent();
    });
  }
}