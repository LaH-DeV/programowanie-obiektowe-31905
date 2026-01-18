namespace Projekt.Models;

using System.Collections.Generic;
using System.Linq;
using Projekt.Models;

public class ShoppingList
{
    public int Id { get; private set; }
    public string Name { get; private set; } = "";

    public List<ShoppingItem> Items { get; private set; } = new();

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

    public void AddItem(ShoppingItem item)
    {
        Items.Add(item);
    }

    public bool RemoveItemByName(string name)
    {
        var item = Items.FirstOrDefault(i => i.Name == name);
        if (item != null)
        {
            Items.Remove(item);
            return true;
        }
        return false;
    }
}
