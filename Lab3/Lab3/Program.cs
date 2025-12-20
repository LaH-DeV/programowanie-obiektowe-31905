using System.Text.Json;
using Lab3.Models;

List<Car> cars;
List<Bike> bikes;

var carsJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "cars.json"));
cars = JsonSerializer.Deserialize<List<Car>>(carsJson) ?? [];

var bikesJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "bikes.json"));
bikes = JsonSerializer.Deserialize<List<Bike>>(bikesJson) ?? [];

var vehicles = new List<Vehicle>();
vehicles.AddRange(cars);
vehicles.AddRange(bikes);

var continueApp = true;
do
{
    Console.WriteLine("--- MENU ---");
    Console.WriteLine("1. Vehicle list");
    Console.WriteLine("2. New vehicle");
    Console.WriteLine("3. Remove vehicle");
    Console.WriteLine("4. Update vehicle");
    Console.WriteLine("0. Exit");
    var option = Console.ReadKey().KeyChar;
    switch (option)
    {
        case '0':
            Console.WriteLine("Bye bye...");
            continueApp = false;
            break;
        case '1':
            ShowVehicles();
            break;
        case '2':
            AddNewVehicle();
            break;
        case '3':
            RemoveVehicle();
            break;
        case '4':
            UpdateVehicle();
            break;
        default:
            Console.WriteLine("Unknown option");
            break;
    }
} while (continueApp);

return;

int GetVehicleType()
{
    Console.WriteLine("1 for car, 2 for bike, 0 to abort");
    var success = int.TryParse(Console.ReadLine(), out var option);
    if (success)
    {
        if (option is >= 0 and <= 2) return option;
    }

    Console.WriteLine("Unknown option");
    return 0;
}


void AddNewVehicle()
{
    switch (GetVehicleType())
    {
        case 1:
            AddNewCar();
            break;
        case 2:
            AddNewBike();
            break;
    }
}

void AddNewBike()
{
    Console.WriteLine("Engine: ");
    var engine = Console.ReadLine();
    Console.WriteLine("Bike type: ");
    var bikeType = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(engine) || string.IsNullOrWhiteSpace(bikeType))
    {
        Console.WriteLine("Invalid engine or bike type");
        return;
    }

    var bike = new Bike(engine, bikeType);
    bikes.Add(bike);
    var json = JsonSerializer.Serialize(bikes);
    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "bikes.json"), json);
    vehicles.Clear();
    vehicles.AddRange(cars);
    vehicles.AddRange(bikes);
}

void AddNewCar()
{
    Console.WriteLine("Model: ");
    var model = Console.ReadLine();
    Console.WriteLine("Engine: ");
    var engine = Console.ReadLine();
    Console.WriteLine("Year: ");
    var success = int.TryParse(Console.ReadLine(), out var year);
    if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(engine) || !success)
    {
        Console.WriteLine("Invalid model, engine or year");
        return;
    }

    var car = new Car(engine, model, year);
    cars.Add(car);
    var json = JsonSerializer.Serialize(cars);
    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "cars.json"), json);
    vehicles.Clear();
    vehicles.AddRange(cars);
    vehicles.AddRange(bikes);
}

void RemoveVehicle()
{
    switch (GetVehicleType())
    {
        case 1:
            RemoveCar();
            break;
        case 2:
            RemoveBike();
            break;
    }
}

void RemoveBike()
{
    Console.WriteLine("Bikes are identified by engine and bike type: ");
    Console.WriteLine("Engine: ");
    var engine = Console.ReadLine();
    Console.WriteLine("Bike type: ");
    var bikeType = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(engine) || string.IsNullOrWhiteSpace(bikeType))
    {
        Console.WriteLine("Invalid engine  or bike type");
        return;
    }

    var bike = bikes.Find(v => v.Engine.Equals(engine) && v.BikeType == bikeType);
    if (bike == null)
    {
        Console.WriteLine("Bike not found");
        return;
    }
    bikes.Remove(bike);
    var json = JsonSerializer.Serialize(bikes);
    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "bikes.json"), json);
    vehicles.Clear();
    vehicles.AddRange(cars);
    vehicles.AddRange(bikes);
}

void RemoveCar()
{
    Console.WriteLine("Cars are identified by model, engine and year: ");
    Console.WriteLine("Model: ");
    var model = Console.ReadLine();
    Console.WriteLine("Engine: ");
    var engine = Console.ReadLine();
    Console.WriteLine("Year: ");
    var success = int.TryParse(Console.ReadLine(), out var year);
    if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(engine) || !success)
    {
        Console.WriteLine("Invalid model, engine or year");
        return;
    }

    var car = cars.Find(c => c.Model.Equals(model) && c.Engine.Equals(engine) && c.Year.Equals(year));
    if (car == null)
    {
        Console.WriteLine("Car not found");
        return;
    }
    cars.Remove(car);
    var json = JsonSerializer.Serialize(cars);
    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "cars.json"), json);
    vehicles.Clear();
    vehicles.AddRange(cars);
    vehicles.AddRange(bikes);
}

void UpdateVehicle()
{
    switch (GetVehicleType())
    {
        case 1:
            UpdateCar();
            break;
        case 2:
            UpdateBike();
            break;
    }
}

void UpdateBike()
{
   
}

void UpdateCar()
{
    Console.WriteLine("Cars are identified by model, engine and year: ");
    Console.WriteLine("Model: ");
    var model = Console.ReadLine();
    Console.WriteLine("Engine: ");
    var engine = Console.ReadLine();
    Console.WriteLine("Year: ");
    var success = int.TryParse(Console.ReadLine(), out var year);
    if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(engine) || !success)
    {
        Console.WriteLine("Invalid model, engine or year");
        return;
    }

    var car = cars.Find(c => c.Model.Equals(model) && c.Engine.Equals(engine) && c.Year.Equals(year));
    if (car == null)
    {
        Console.WriteLine("Car not found");
        return;
    }

    var edit = true;
    var didChange = false;
    do
    {
        Console.WriteLine("--- EDIT CAR MENU ---");
        Console.WriteLine("1. Edit model");
        Console.WriteLine("2. Edit engine");
        Console.WriteLine("3. Edit year");
        Console.WriteLine("0. Cancel");
        var option = Console.ReadKey().KeyChar;
        switch (option)
        {
            case '0':
                edit = false;
                break;
            case '1':
                Console.WriteLine("Model: ");
                var newModel = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newModel))
                {
                    Console.WriteLine("Invalid model, engine or year");
                    break;
                }
                car.setModel(newModel);
                break;
            case '2':
                var newEngine = Console.ReadLine();
                if (newEngine != car.Engine)
                    //
                    break;
            default:
                Console.WriteLine("Unknown option");
                break;
        }
    } while (edit);
    if (!didChange) return;
    var json = JsonSerializer.Serialize(cars);
    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "cars.json"), json);
    vehicles.Clear();
    vehicles.AddRange(cars);
    vehicles.AddRange(bikes);
}

void ShowVehicles()
{
    Console.WriteLine("List of vehicles");
    foreach (var vehicle in vehicles)
    {
        vehicle.ShowMe();
    }
}