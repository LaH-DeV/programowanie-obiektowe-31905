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
        Console.WriteLine("Bike is starting");
        base.Start();
    }

    public override void ShowMe()
    {
        Console.WriteLine($"Bike of type: {BikeType}, engine: {Engine}");
    }

    public void SetBikeType(string bikeType)
    {
        BikeType = bikeType;
    }

    public void SetEngine(string engine)
    {
        Engine = engine;
    }
}