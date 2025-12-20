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
        Console.WriteLine($"Model: {Model}, Year: {year}, Engine: {Engine}");
    }

    public void setModel(string model)
    {
        Model = model;
    }
}