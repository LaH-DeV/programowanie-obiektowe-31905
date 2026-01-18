namespace Projekt.Models;

public class ShoppingItem : BaseItem
{
    public int Id { get; private set; }
    public bool IsBought { get; private set; }

    public int ShoppingListId { get; private set; }
    public ShoppingList ShoppingList { get; private set; } = null!;

    // EF Core
    protected ShoppingItem() : base("", 0) { }

    public ShoppingItem(string name, int quantity, int shoppingListId) : base(name, quantity)
    {
        SetName(name);
        SetQuantity(quantity);
        MarkAsUnbought();
        ShoppingListId = shoppingListId;
    }

    public override void Update(string name, int quantity)
    {
        SetName(name);
        SetQuantity(quantity);
    }

    public void MarkAsBought()
    {
        IsBought = true;
    }

    public void MarkAsUnbought()
    {
        IsBought = false;
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nazwa nie może być pusta");

        Name = name;
    }

    private void SetQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Ilość musi być większa od zera");

        Quantity = quantity;
    }
}
