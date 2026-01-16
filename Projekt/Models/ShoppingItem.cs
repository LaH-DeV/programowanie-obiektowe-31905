namespace Projekt.Models;

public class ShoppingItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public bool IsBought { get; set; }

    // relacja
    public int ShoppingListId { get; set; }
    public ShoppingList ShoppingList { get; set; } = null!;
}
