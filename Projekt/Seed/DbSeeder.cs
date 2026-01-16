using Projekt.Data;
using Projekt.Models;

namespace Projekt.Seed;

public static class DbSeeder {
  public static void Seed(IServiceProvider services) {
    using
    var scope = services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService < AppDbContext > ();

    db.Database.EnsureCreated();

    if (db.Lists.Any()) return;

    var weeklyList = new ShoppingList {
      Name = "Zakupy tygodniowe",
        Items = new List < ShoppingItem > {
          new() {
            Name = "Chleb", Quantity = 2, IsBought = true
          },
          new() {
            Name = "Mleko", Quantity = 3, IsBought = false
          },
          new() {
            Name = "Jajka", Quantity = 10, IsBought = false
          },
        }
    };

    var partyList = new ShoppingList {
      Name = "Impreza urodzinowa",
        Items = new List < ShoppingItem > {
          new() {
            Name = "Pizza mrożona", Quantity = 4, IsBought = false
          },
          new() {
            Name = "Cola", Quantity = 6, IsBought = true
          },
          new() {
            Name = "Chipsy", Quantity = 5, IsBought = false
          },
        }
    };

    db.Lists.AddRange(weeklyList, partyList);
    db.SaveChanges();
  }
}