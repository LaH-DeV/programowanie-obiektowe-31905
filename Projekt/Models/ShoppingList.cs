namespace Projekt.Models;

public class ShoppingList
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public List<ShoppingItem> Items { get; set; } = [];
}
