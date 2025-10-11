// Zadanie 1. Warunki
const int requiredAgeEntry = 14;
const int requiredAgeBuySim = 18;
const string accessDeniedEntryMessage = "You have to be at least 14 years old to enter my shop";
const string accessDeniedBuySim = "You have to be at least 18 years old to buy sim card. Welcome";
const string accessAllowed = "Welcome to my shop";

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
        Console.WriteLine(accessDeniedBuySim);
        break;
    default:
        Console.WriteLine(accessAllowed);
        break;
}
