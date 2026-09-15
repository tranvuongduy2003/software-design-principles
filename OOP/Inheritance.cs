using System;

namespace SoftwareDesignPrinciples.OOP
{
    /// <summary>
    /// Inheritance allows a class to inherit fields and methods from another class.
    /// The 'base' class provides common functionality, and 'derived' classes specialize it.
    /// </summary>
    public static class InheritanceDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Inheritance Demo ---");

            Manager manager = new Manager("Alice", 80000, 5);
            manager.Work();
            manager.ConductMeeting();
            
            Console.WriteLine();

            Developer dev = new Developer("Bob", 60000, "C#");
            dev.Work();
            dev.WriteCode();
            
            Console.WriteLine();
        }
    }

    // Base class
    public class Employee
    {
        public string Name { get; set; }
        public decimal Salary { get; set; }

        public Employee(string name, decimal salary)
        {
            Name = name;
            Salary = salary;
            Console.WriteLine($"[Employee] Constructor called for {Name}");
        }

        // Virtual method allows overriding in derived classes
        public virtual void Work()
        {
            Console.WriteLine($"{Name} is doing general employee work.");
        }
    }

    // Derived class
    public class Manager : Employee
    {
        public int TeamSize { get; set; }

        // Constructor chaining using base()
        public Manager(string name, decimal salary, int teamSize) 
            : base(name, salary)
        {
            TeamSize = teamSize;
            Console.WriteLine($"[Manager] Constructor called for {Name}");
        }

        // Overriding the base method
        public override void Work()
        {
            Console.WriteLine($"{Name} is managing a team of {TeamSize} people.");
        }

        public void ConductMeeting()
        {
            Console.WriteLine($"{Name} is conducting a meeting.");
        }
    }

    // Sealed derived class - cannot be inherited further
    public sealed class Developer : Employee
    {
        public string ProgrammingLanguage { get; set; }

        public Developer(string name, decimal salary, string programmingLanguage) 
            : base(name, salary)
        {
            ProgrammingLanguage = programmingLanguage;
            Console.WriteLine($"[Developer] Constructor called for {Name}");
        }

        public override void Work()
        {
            // Call base class implementation
            base.Work();
            Console.WriteLine($"{Name} is also writing {ProgrammingLanguage} code.");
        }

        public void WriteCode()
        {
            Console.WriteLine($"{Name} is compiling code...");
        }
    }
}
