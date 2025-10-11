// Zadanie 1. Warunki
const int requiredAgeEntry = 14;
const int requiredAgeBuySim = 18;
const string accessDeniedEntryMessage = "You have to be at least 14 years old to enter my shop";
const string accessAllowedWithoutBuySimMessage = "You have to be at least 18 years old to buy sim card. Welcome";
const string accessAllowedMessage = "Welcome to my shop";

var age = 0;
var success = false;

while (!success) {
    Console.WriteLine("Enter your age:");
    var input = Console.ReadLine();
    success = int.TryParse(input, out age);
}

switch (age)
{
    case < requiredAgeEntry:
        Console.WriteLine(accessDeniedEntryMessage);
        break;
    case < requiredAgeBuySim:
        Console.WriteLine(accessAllowedWithoutBuySimMessage);
        break;
    default:
        Console.WriteLine(accessAllowedMessage);
        break;
}

// Zadanie 2. Pętle
var count = 0;
while (count < 5)
{
    Console.WriteLine($"Iteration number: {count}");
    count++;
}

string? password;
do
{
    Console.WriteLine("Enter your password:");
    password = Console.ReadLine();
} while (password != "admin123");
Console.WriteLine("Logged in successfully");

var number = 0;
while (number <= 0)
{
    Console.WriteLine("Enter positive number:");
    number = int.Parse(Console.ReadLine()!);
}


var fruits = new []{"apple", "banana", "peach"};
foreach (var fruit in fruits)
{
    Console.WriteLine($"Fruit: {fruit}");
}

var cities = new []{"Poznań", "Warszawa", "Gdańsk", "Wrocław", "Kraków"};
foreach (var city in cities)
{
    Console.WriteLine(city);
}