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
do {
    var option = GetMenuOption("--- MENU ---", ["Exit", "Vehicle list", "New vehicle", "Remove vehicle", "Update vehicle"]);
    switch (option) {
        case 0: {
            Console.WriteLine("Bye bye...");
            continueApp = false;
            break;
        }
        case 1: {
            Console.WriteLine("List of vehicles");
            foreach (var vehicle in vehicles) vehicle.ShowMe();
            break;
        }
        case 2: {
            var type = GetVehicleType();
            if (type == 0) break;
            else if (type == 1) {
                var (model, engine, year) = GetCarInfo();
                if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(engine) || year == null) break;
                AddNewVehicle(new Car(engine, model, year.Value), cars, "cars.json");
            } else if (type == 2) {
                var (engine, bikeType) = GetBikeInfo();
                if (string.IsNullOrWhiteSpace(engine) || string.IsNullOrWhiteSpace(bikeType)) break;
                AddNewVehicle(new Bike(engine, bikeType), bikes, "bikes.json");
            }
            break;
        }
        case 3: {
            var type = GetVehicleType();
            if (type == 0) break;
            if (type == 1) {
                var car = FindCar();
                if (car != null) RemoveVehicle(car, cars, "cars.json");
            }
            if (type == 2) {
                var bike = FindBike();
                if (bike != null) RemoveVehicle(bike, bikes, "bikes.json");
            }
            break;
        }
        case 4: {
            var type = GetVehicleType();
            if (type == 0) break;
            if (type == 1) {
                var car = FindCar();
                if (car == null) break;
                EditVehicle(car,
                    ["Model", "Engine", "Year"],
                    [
                        (c, v) => c.SetModel(v),
                        (c, v) => c.SetEngine(v),
                        (c, v) => c.SetYear(int.Parse(v))
                    ],
                    cars,
                    "cars.json"
                );
            }
            if (type == 2) {
                var bike = FindBike();
                if (bike == null) break;
                EditVehicle(bike,
                    ["Engine", "Bike type"],
                    [
                        (b, v) => b.SetEngine(v),
                        (b, v) => b.SetBikeType(v)
                    ],
                    bikes,
                    "bikes.json"
                );
            }
            break;
        }
    }
} while (continueApp);

return;

void AddNewVehicle<T>(T vehicle, List<T> list, string filePath) where T : Vehicle {
    list.Add(vehicle);
    SaveVehicles(list, filePath, $"{typeof(T).Name} added");
}

void RemoveVehicle<T>(T vehicle, List<T> list, string filePath) where T : Vehicle {
    list.Remove(vehicle);
    SaveVehicles(list, filePath, $"{typeof(T).Name} removed");
}

Car? FindCar() {
    Console.WriteLine("Cars are identified by model, engine and year: ");
    var (model, engine, year) = GetCarInfo();
    if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(engine) || year == null) return null;
    var car = cars.Find(c => c.Model.Equals(model) && c.Engine.Equals(engine) && c.Year.Equals(year));
    if (car == null) Console.WriteLine("Car not found");
    return car;
}

Bike? FindBike() {
    Console.WriteLine("Bikes are identified by engine and bike type: ");
    var (engine, bikeType) = GetBikeInfo();
    if (string.IsNullOrWhiteSpace(engine) || string.IsNullOrWhiteSpace(bikeType)) return null;
    var bike = bikes.Find(v => v.Engine.Equals(engine) && v.BikeType == bikeType);
    if (bike == null) Console.WriteLine("Bike not found");
    return bike;
}

(string? model, string? engine, int? year) GetCarInfo() {
    var model = ReadString("Model: ");
    var engine = ReadString("Engine: ");
    var year = ReadInt("Year: ");
    if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(engine) || year == null) {
        Console.WriteLine("Invalid model, engine or year");
        return (null, null, null);
    }
    return (model, engine, year);
}

(string? engine, string? bikeType) GetBikeInfo() {
    var engine = ReadString("Engine: ");
    var bikeType = ReadString("Bike type: ");
    if (string.IsNullOrWhiteSpace(engine) || string.IsNullOrWhiteSpace(bikeType)) {
        Console.WriteLine("Invalid engine or bike type");
        return (null, null);
    }
    return (engine, bikeType);
}

string? ReadString(string prompt) {
    Console.WriteLine(prompt);
    return Console.ReadLine();
}

int? ReadInt(string prompt) {
    Console.WriteLine(prompt);
    var success = int.TryParse(Console.ReadLine(), out var value);
    if (success) return value;
    return null;
}

int GetVehicleType() {
    var option = GetMenuOption("Select vehicle type", ["Abort", "Car", "Bike"]);
    if (option is >= 0 and <= 2) return option;
    return 0;
}

int GetMenuOption(string title, string[] options) {
    Console.WriteLine(title);
    for (var i = 0; i < options.Length; i++) {
        Console.WriteLine($"{i}. {options[i]}");
    }
    var success = int.TryParse(Console.ReadKey().KeyChar.ToString(), out var option);
    Console.WriteLine();
    if (success) {
        if (option >= 0 && option < options.Length) return option;
    }
    Console.WriteLine("Unknown option");
    return -1;
}

void EditVehicle<T>(T vehicle, string[] fields, Action<T, string>[] setters, List<T> list, string filePath) where T : Vehicle {
    bool edit = true;
    bool didChange = false;
    do {
        int option = GetMenuOption("--- EDIT MENU ---", [.. fields.Prepend("Done / Cancel")]);
        if (option < 0 || option > fields.Length) {
            Console.WriteLine("Valid option required");
            continue;
        }
        if (option == 0) { edit = false; continue; }
        string? newValue = ReadString(fields[option - 1] + ": ");
        if (string.IsNullOrWhiteSpace(newValue)) {
            Console.WriteLine("Invalid input");
            continue;
        }
        setters[option - 1](vehicle, newValue);
        didChange = true;
    } while (edit);

    if (didChange) SaveVehicles(list, filePath, $"{typeof(T).Name} updated");
    else Console.WriteLine("No changes made");
}


void SaveVehicles<T>(List<T> list, string filePath, string successMessage) where T : Vehicle {
    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), filePath), JsonSerializer.Serialize(list));
    vehicles.Clear();
    vehicles.AddRange(cars);
    vehicles.AddRange(bikes);
    Console.WriteLine(successMessage);
}

