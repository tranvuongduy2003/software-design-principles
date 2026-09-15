using System;

namespace SoftwareDesignPrinciples.DesignPatterns.StructuralPatterns
{
    /// <summary>
    /// Decorator Pattern
    /// Attaches additional responsibilities to an object dynamically.
    /// Decorators provide a flexible alternative to subclassing for extending functionality.
    /// </summary>
    public static class DecoratorPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Decorator Pattern Demo ---");
            Console.WriteLine("Scenario: Coffee shop with base coffee and add-ons.\n");

            ICoffee myCoffee = new SimpleCoffee();
            Console.WriteLine($"{myCoffee.GetDescription()} - ${myCoffee.GetCost()}");

            myCoffee = new MilkDecorator(myCoffee);
            Console.WriteLine($"{myCoffee.GetDescription()} - ${myCoffee.GetCost()}");

            myCoffee = new SugarDecorator(myCoffee);
            Console.WriteLine($"{myCoffee.GetDescription()} - ${myCoffee.GetCost()}");

            myCoffee = new WhipCreamDecorator(myCoffee);
            Console.WriteLine($"{myCoffee.GetDescription()} - ${myCoffee.GetCost()}");
            
            Console.WriteLine();
        }
    }

    // Component
    public interface ICoffee
    {
        string GetDescription();
        double GetCost();
    }

    // Concrete Component
    public class SimpleCoffee : ICoffee
    {
        public string GetDescription() => "Simple Coffee";
        public double GetCost() => 2.0;
    }

    // Base Decorator
    public abstract class CoffeeDecorator : ICoffee
    {
        protected ICoffee _coffee;

        public CoffeeDecorator(ICoffee coffee)
        {
            _coffee = coffee;
        }

        public virtual string GetDescription() => _coffee.GetDescription();
        public virtual double GetCost() => _coffee.GetCost();
    }

    // Concrete Decorators
    public class MilkDecorator : CoffeeDecorator
    {
        public MilkDecorator(ICoffee coffee) : base(coffee) { }

        public override string GetDescription() => base.GetDescription() + ", Milk";
        public override double GetCost() => base.GetCost() + 0.5;
    }

    public class SugarDecorator : CoffeeDecorator
    {
        public SugarDecorator(ICoffee coffee) : base(coffee) { }

        public override string GetDescription() => base.GetDescription() + ", Sugar";
        public override double GetCost() => base.GetCost() + 0.2;
    }

    public class WhipCreamDecorator : CoffeeDecorator
    {
        public WhipCreamDecorator(ICoffee coffee) : base(coffee) { }

        public override string GetDescription() => base.GetDescription() + ", Whip Cream";
        public override double GetCost() => base.GetCost() + 0.7;
    }
}
