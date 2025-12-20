namespace Lab3.Models;

public class Bike : Vehicle
{
    public string BikeType { get; protected set; }

    public Bike(string engine, string bikeType) : base(engine)
    {
        BikeType = bikeType;
    }

    public override void Start()
    {
        Console.WriteLine("Unlocked!");
        base.Start();
    }

    public override void ShowMe()
    {
        Console.WriteLine($"Engine: {Engine},  Bike: {BikeType}");
    }
}