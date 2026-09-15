using SoftwareDesignPrinciples.OOP;
using SoftwareDesignPrinciples.SOLID;
using SoftwareDesignPrinciples.DesignPatterns.CreationalPatterns;
using SoftwareDesignPrinciples.DesignPatterns.StructuralPatterns;
using SoftwareDesignPrinciples.DesignPatterns.BehavioralPatterns;

// ============================================================
//  Learn OOP, SOLID & Design Patterns — Interactive Menu
// ============================================================
//  Run the project and pick any topic by number.
//  Each demo prints explanatory output to the console.
//  Set a breakpoint inside any Run() method to debug step-by-step.
// ============================================================

var demos = new (string Category, string Name, Action Run)[]
{
    // ── OOP ──────────────────────────────────────────────────
    ("OOP",                  "Đóng gói (Encapsulation)",      EncapsulationDemo.Run),
    ("OOP",                  "Trừu tượng (Abstraction)",     AbstractionDemo.Run),
    ("OOP",                  "Kế thừa (Inheritance)",        InheritanceDemo.Run),
    ("OOP",                  "Đa hình (Polymorphism)",       PolymorphismDemo.Run),

    // ── SOLID ────────────────────────────────────────────────
    ("SOLID",                "Single Responsibility (SRP)",  SingleResponsibilityDemo.Run),
    ("SOLID",                "Open/Closed (OCP)",            OpenClosedDemo.Run),
    ("SOLID",                "Liskov Substitution (LSP)",    LiskovSubstitutionDemo.Run),
    ("SOLID",                "Interface Segregation (ISP)",  InterfaceSegregationDemo.Run),
    ("SOLID",                "Dependency Inversion (DIP)",   DependencyInversionDemo.Run),

    // ── Creational Patterns ──────────────────────────────────
    ("Creational Patterns",  "Singleton",                    SingletonPatternDemo.Run),
    ("Creational Patterns",  "Factory Method",               FactoryMethodPatternDemo.Run),
    ("Creational Patterns",  "Abstract Factory",             AbstractFactoryPatternDemo.Run),
    ("Creational Patterns",  "Builder",                      BuilderPatternDemo.Run),
    ("Creational Patterns",  "Prototype",                    PrototypePatternDemo.Run),

    // ── Structural Patterns ──────────────────────────────────
    ("Structural Patterns",  "Adapter",                      AdapterPatternDemo.Run),
    ("Structural Patterns",  "Bridge",                       BridgePatternDemo.Run),
    ("Structural Patterns",  "Composite",                    CompositePatternDemo.Run),
    ("Structural Patterns",  "Decorator",                    DecoratorPatternDemo.Run),
    ("Structural Patterns",  "Facade",                       FacadePatternDemo.Run),
    ("Structural Patterns",  "Flyweight",                    FlyweightPatternDemo.Run),
    ("Structural Patterns",  "Proxy",                        ProxyPatternDemo.Run),

    // ── Behavioral Patterns ──────────────────────────────────
    ("Behavioral Patterns",  "Chain of Responsibility",      ChainOfResponsibilityPatternDemo.Run),
    ("Behavioral Patterns",  "Command",                      CommandPatternDemo.Run),
    ("Behavioral Patterns",  "Iterator",                     IteratorPatternDemo.Run),
    ("Behavioral Patterns",  "Mediator",                     MediatorPatternDemo.Run),
    ("Behavioral Patterns",  "Memento",                      MementoPatternDemo.Run),
    ("Behavioral Patterns",  "Observer",                     ObserverPatternDemo.Run),
    ("Behavioral Patterns",  "State",                        StatePatternDemo.Run),
    ("Behavioral Patterns",  "Strategy",                     StrategyPatternDemo.Run),
    ("Behavioral Patterns",  "Template Method",              TemplateMethodPatternDemo.Run),
    ("Behavioral Patterns",  "Visitor",                      VisitorPatternDemo.Run),
};

while (true)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
    Console.WriteLine("║      Learn OOP, SOLID & Design Patterns (C# / .NET)    ║");
    Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
    Console.WriteLine();

    string? currentCategory = null;
    for (int i = 0; i < demos.Length; i++)
    {
        if (demos[i].Category != currentCategory)
        {
            currentCategory = demos[i].Category;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  ── {currentCategory} ──");
            Console.ResetColor();
        }
        Console.WriteLine($"  {i + 1,2}. {demos[i].Name}");
    }

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("   0. Run ALL demos sequentially");
    Console.WriteLine("   q. Quit");
    Console.ResetColor();
    Console.WriteLine();
    Console.Write("  Enter your choice: ");

    var input = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(input) || input.Equals("q", StringComparison.OrdinalIgnoreCase))
        break;

    if (input == "0")
    {
        foreach (var (category, name, run) in demos)
        {
            PrintHeader(category, name);
            run();
            Console.WriteLine();
        }
    }
    else if (int.TryParse(input, out int choice) && choice >= 1 && choice <= demos.Length)
    {
        var (category, name, run) = demos[choice - 1];
        PrintHeader(category, name);
        run();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  Invalid choice: '{input}'");
        Console.ResetColor();
    }

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  Press any key to return to the menu...");
    Console.ResetColor();
    Console.ReadKey(true);
}

static void PrintHeader(string category, string name)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"  ━━━ [{category}] {name} ━━━");
    Console.ResetColor();
    Console.WriteLine();
}
