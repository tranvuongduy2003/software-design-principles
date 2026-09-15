using System;

namespace SoftwareDesignPrinciples.DesignPatterns.CreationalPatterns;

/// <summary>
/// Prototype Pattern
/// Specifies the kinds of objects to create using a prototypical instance, and creates new objects by copying this prototype.
/// </summary>
public static class PrototypePatternDemo
{
    public static void Run()
    {
        Console.WriteLine("\n--- Prototype Pattern Demo ---");

        var originalCharacter = new GameCharacter
        {
            Name = "Warrior",
            Health = 100,
            Weapon = new Weapon { Name = "Sword", Damage = 20 }
        };

        Console.WriteLine("Original Character:");
        Console.WriteLine(originalCharacter);

        var shallowClone = originalCharacter.ShallowClone();
        var deepClone = originalCharacter.DeepClone();

        Console.WriteLine("\nModifying Original Character's Weapon...");
        originalCharacter.Weapon.Name = "Axe";
        originalCharacter.Weapon.Damage = 30;

        Console.WriteLine("\nAfter modification:");
        Console.WriteLine("Original: " + originalCharacter);
        Console.WriteLine("Shallow Clone (Affected by original's weapon change): " + shallowClone);
        Console.WriteLine("Deep Clone (Unaffected): " + deepClone);
    }
}

public interface IPrototype<T>
{
    T ShallowClone();
    T DeepClone();
}

public class Weapon
{
    public string Name { get; set; } = string.Empty;
    public int Damage { get; set; }

    public override string ToString() => $"{Name} (Dmg: {Damage})";
}

public class GameCharacter : IPrototype<GameCharacter>
{
    public string Name { get; set; } = string.Empty;
    public int Health { get; set; }
    public Weapon Weapon { get; set; } = new();

    public GameCharacter ShallowClone()
    {
        return (GameCharacter)this.MemberwiseClone();
    }

    public GameCharacter DeepClone()
    {
        var clone = (GameCharacter)this.MemberwiseClone();
        clone.Weapon = new Weapon { Name = this.Weapon.Name, Damage = this.Weapon.Damage };
        return clone;
    }

    public override string ToString()
    {
        return $"[Character: {Name}, Health: {Health}, Weapon: {Weapon}]";
    }
}
