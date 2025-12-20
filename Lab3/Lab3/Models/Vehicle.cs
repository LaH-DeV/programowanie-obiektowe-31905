namespace Lab3.Models;

public abstract class Vehicle
{
    public bool IsOn { get; private set; }

    public string Engine { get; protected set; }

    public Vehicle(string engine)
    {
        Engine = engine;
    }

    public virtual void Start()
    {
        IsOn = true;
    }

    public void Stop()
    {
        IsOn = false;
    }

    public virtual void ShowMe()
    {
        Console.WriteLine($"Engine: {Engine}");
    }
}