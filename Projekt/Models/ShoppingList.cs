namespace Projekt.Models;

public class ShoppingList
{
    public int Id { get; private set; }
    public string Name { get; private set; } = "";

    // EF Core
    protected ShoppingList() { }

    public ShoppingList(string name)
    {
        SetName(name);
    }

    public void Rename(string name)
    {
        SetName(name);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nazwa listy nie może być pusta");

        Name = name;
    }

    public void UpdateName(string name)
    {
        SetName(name);
    }
}
