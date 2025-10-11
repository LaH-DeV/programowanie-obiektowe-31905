// Klasa i obiekt


var me = new Person("Przemysław", 29);
me.IntroduceYourself();
var col = new Person("Mateusz", 22);
col.IntroduceYourself();


var bankaccount = new BankAccount(250000);
bankaccount.Deposit(250);
bankaccount.Withdraw(250);
Console.WriteLine(bankaccount.ReadBalance());


class Person(string firstName, int age)
{
    private string firstName = firstName;
    private int age = age;

    public void IntroduceYourself()
    {
        var message = age > 25 ? "young!" : "old.";
        Console.WriteLine($"Hello, my name is {firstName}, I am {age} years {message}");
    }
}


class BankAccount(double balance)
{
    private double balance  = balance;
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
}

class Dog : Animal
{
    public void Bark() => Console.WriteLine("Woof woof!");
}

class Cat : Animal
{
    public void Meow() => Console.WriteLine("Meow meow!");
}

