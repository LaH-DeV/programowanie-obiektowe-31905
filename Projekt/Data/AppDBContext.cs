using Microsoft.EntityFrameworkCore;
using Projekt.Models;

namespace Projekt.Data;

public class AppDbContext : DbContext
{
    public DbSet<ShoppingList> Lists => Set<ShoppingList>();
    public DbSet<ShoppingItem> Items => Set<ShoppingItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
