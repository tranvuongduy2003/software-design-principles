using System;

namespace SoftwareDesignPrinciples.DesignPatterns.CreationalPatterns;

/// <summary>
/// Singleton Pattern
/// Ensures a class has only one instance and provides a global point of access to it.
/// </summary>
public static class SingletonPatternDemo
{
    public static void Run()
    {
        Console.WriteLine("\n--- Singleton Pattern Demo ---");
        Console.WriteLine("Creating Logger instances...");
        
        var logger1 = Logger.Instance;
        var logger2 = Logger.Instance;

        logger1.Log("Message from logger1");
        logger2.Log("Message from logger2");

        Console.WriteLine($"Are logger1 and logger2 the same instance? {ReferenceEquals(logger1, logger2)}");

        Console.WriteLine("\nNote: Singleton is appropriate for managing a shared resource like a configuration, logger, or connection pool.");
        Console.WriteLine("However, it can be an anti-pattern when overused, as it introduces global state and hides dependencies, making testing difficult.");
    }
}

public sealed class Logger
{
    private static readonly Lazy<Logger> _instance = new(() => new Logger());

    private Logger()
    {
        Console.WriteLine("Logger initialized. (This should only happen once)");
    }

    public static Logger Instance => _instance.Value;

    public void Log(string message)
    {
        Console.WriteLine($"[Log]: {message}");
    }
}
