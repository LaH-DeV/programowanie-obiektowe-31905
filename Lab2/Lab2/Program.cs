// Klasa i obiekt


var me = new Person("Przemysław", 29);
me.IntroduceYourself();
var col = new Person("Mateusz", 22);
col.IntroduceYourself();


var bankaccount = new BankAccount(250000);
bankaccount.Deposit(250);
bankaccount.Withdraw(250);
Console.WriteLine(bankaccount.ReadBalance());

//
Animal[] animals = { new Dog(), new Cat() };
foreach (var animal in animals)
{
    animal.MakeSound();
}
//

class Person
{
    private string firstName;
    private int age;

    public Person(string firstName, int age)
    {
        this.firstName = firstName;
        this.age = age;
    }

    public void IntroduceYourself()
    {
        var message = age > 25 ? "young!" : "old.";
        Console.WriteLine($"Hello, my name is {firstName}, I am {age} years {message}");
    }
}


class BankAccount
{
    private double balance;

    public BankAccount(double balance)
    {
        this.balance = balance;
    }

    public void Deposit(double amount) { balance += amount; }
    public double ReadBalance() { return balance; }

    public void Withdraw(double amount)
    {
        if (balance >= amount)
        {
            balance -= amount;
        }
        else
        {
            throw new Exception("Insufficient balance");
        }
    }
}

class Animal
{
    public void Eat() => Console.WriteLine("The animal is eating...");
    public virtual void MakeSound() => Console.WriteLine("The animal makes a sound...");
}

class Dog : Animal
{
    public override void MakeSound() => Console.WriteLine("Woof woof!");
}

class Cat : Animal
{
    public override void MakeSound() => Console.WriteLine("Meow meow!");
}


class Vehicle
{
    public virtual void Start() => Console.WriteLine("The vehicle is starting...");
}

class Car : Vehicle
{
    public override void Start() => Console.WriteLine("The car is starting...");
    public void Move() => Console.WriteLine("The car is moving...");
}

class ElectricCar : Car
{
    public void Charge() => Console.WriteLine("Battery is charging...");
}
