namespace Lab3.Models;

public class Car : Vehicle
{
    private int year;
    public string Model { get; protected set; }

    public int Year
    {
        get => year;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, 2000);
            year = value;
        }
    }

    public Car(string engine, string model, int year) : base(engine)
    {
        Model = model;
        Year = year;
    }

    public override void ShowMe()
    {
        Console.WriteLine($"Car of model: {Model}, engine: {Engine}, year: {Year}");
    }

    public void SetModel(string model)
    {
        Model = model;
    }

    public void SetEngine(string engine)
    {
        base.Engine = engine;
    }

    public void SetYear(int year)
    {
        Year = year;
    }
}