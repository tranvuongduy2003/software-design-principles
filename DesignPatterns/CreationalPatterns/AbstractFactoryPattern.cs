using System;

namespace SoftwareDesignPrinciples.DesignPatterns.CreationalPatterns;

/// <summary>
/// Abstract Factory Pattern
/// Provides an interface for creating families of related or dependent objects without specifying their concrete classes.
/// </summary>
public static class AbstractFactoryPatternDemo
{
    public static void Run()
    {
        Console.WriteLine("\n--- Abstract Factory Pattern Demo ---");

        Console.WriteLine("Creating Windows UI:");
        CreateUI(new WindowsFactory());

        Console.WriteLine("\nCreating Mac UI:");
        CreateUI(new MacFactory());
    }

    private static void CreateUI(IUIFactory factory)
    {
        var button = factory.CreateButton();
        var checkbox = factory.CreateCheckbox();

        button.Paint();
        checkbox.Render();
    }
}

public interface IButton { void Paint(); }
public interface ICheckbox { void Render(); }

public interface IUIFactory
{
    IButton CreateButton();
    ICheckbox CreateCheckbox();
}

public class WindowsButton : IButton
{
    public void Paint() => Console.WriteLine("Painting a Windows Button.");
}
public class WindowsCheckbox : ICheckbox
{
    public void Render() => Console.WriteLine("Rendering a Windows Checkbox.");
}

public class MacButton : IButton
{
    public void Paint() => Console.WriteLine("Painting a Mac Button.");
}
public class MacCheckbox : ICheckbox
{
    public void Render() => Console.WriteLine("Rendering a Mac Checkbox.");
}

public class WindowsFactory : IUIFactory
{
    public IButton CreateButton() => new WindowsButton();
    public ICheckbox CreateCheckbox() => new WindowsCheckbox();
}

public class MacFactory : IUIFactory
{
    public IButton CreateButton() => new MacButton();
    public ICheckbox CreateCheckbox() => new MacCheckbox();
}
