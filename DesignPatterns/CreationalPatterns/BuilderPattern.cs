using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.DesignPatterns.CreationalPatterns;

/// <summary>
/// Builder Pattern
/// Separates the construction of a complex object from its representation so that the same construction process can create different representations.
/// </summary>
public static class BuilderPatternDemo
{
    public static void Run()
    {
        Console.WriteLine("\n--- Builder Pattern Demo ---");

        Console.WriteLine("Building a custom pizza:");
        var customPizza = new PizzaBuilder()
            .SetSize("Large")
            .SetCrust("Thin")
            .AddTopping("Cheese")
            .AddTopping("Pepperoni")
            .AddTopping("Mushrooms")
            .Build();
        
        Console.WriteLine(customPizza);

        Console.WriteLine("\nUsing Director to build a preset Margherita pizza:");
        var director = new PizzaDirector();
        var margherita = director.ConstructMargherita(new PizzaBuilder());
        Console.WriteLine(margherita);
    }
}

public class Pizza
{
    public string Size { get; set; } = "Medium";
    public string Crust { get; set; } = "Regular";
    public List<string> Toppings { get; } = new();

    public override string ToString()
    {
        return $"Pizza [Size={Size}, Crust={Crust}, Toppings={string.Join(", ", Toppings)}]";
    }
}

public class PizzaBuilder
{
    private readonly Pizza _pizza = new();

    public PizzaBuilder SetSize(string size)
    {
        _pizza.Size = size;
        return this;
    }

    public PizzaBuilder SetCrust(string crust)
    {
        _pizza.Crust = crust;
        return this;
    }

    public PizzaBuilder AddTopping(string topping)
    {
        _pizza.Toppings.Add(topping);
        return this;
    }

    public Pizza Build()
    {
        return _pizza;
    }
}

public class PizzaDirector
{
    public Pizza ConstructMargherita(PizzaBuilder builder)
    {
        return builder
            .SetSize("Medium")
            .SetCrust("Thin")
            .AddTopping("Tomato Sauce")
            .AddTopping("Mozzarella")
            .AddTopping("Basil")
            .Build();
    }
}
