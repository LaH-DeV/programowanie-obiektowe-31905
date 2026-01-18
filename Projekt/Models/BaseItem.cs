namespace Projekt.Models;

public abstract class BaseItem
{
    public string Name { get; protected set; }
    public int Quantity { get; protected set; }

    protected BaseItem(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
    }

    public abstract void Update(string name, int quantity);
}