using System;

namespace SoftwareDesignPrinciples.OOP
{
    /// <summary>
    /// Abstraction is the process of hiding the implementation details and showing only the essential features of the object.
    /// Can be achieved using abstract classes and interfaces.
    /// </summary>
    public static class AbstractionDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Abstraction Demo ---");

            // We cannot instantiate interfaces or abstract classes directly:
            // IVehicle v = new IVehicle(); // Error
            // AbstractVehicle av = new AbstractVehicle(); // Error

            // Program to an interface/abstract class
            IVehicle myCar = new Car("Toyota Camry");
            myCar.StartEngine();
            myCar.Move();
            myCar.StopEngine();
            
            Console.WriteLine();

            AbstractVehicle myMotorcycle = new Motorcycle("Yamaha R1");
            myMotorcycle.StartEngine();
            myMotorcycle.Move();
            myMotorcycle.Honk(); // From abstract base class
            myMotorcycle.StopEngine();
            
            Console.WriteLine();
        }
    }

    // Interface: Defines a strict contract, no implementation details (prior to C# 8)
    // Used when classes share behavior but aren't conceptually related by an "is-a" hierarchy
    public interface IVehicle
    {
        void StartEngine();
        void StopEngine();
        void Move();
    }

    // Abstract Class: Can provide partial implementation and state
    // Used when classes are conceptually related and share common implementation
    public abstract class AbstractVehicle : IVehicle
    {
        public string Model { get; set; }
        protected bool IsEngineRunning { get; set; }

        protected AbstractVehicle(string model)
        {
            Model = model;
        }

        // Implementing interface methods, but can mark them virtual or leave as is
        public void StartEngine()
        {
            IsEngineRunning = true;
            Console.WriteLine($"{Model} engine started.");
            PlayEngineSound();
        }

        public void StopEngine()
        {
            IsEngineRunning = false;
            Console.WriteLine($"{Model} engine stopped.");
        }

        // Common functionality provided by the base abstract class
        public void Honk()
        {
            Console.WriteLine($"{Model} says: Beep beep!");
        }

        // Abstract method: Must be implemented by derived classes
        public abstract void Move();

        // Protected abstract method: internal behavior specific to derived classes
        protected abstract void PlayEngineSound();
    }

    // Concrete Class
    public class Car : AbstractVehicle
    {
        public Car(string model) : base(model)
        {
        }

        public override void Move()
        {
            if (IsEngineRunning)
                Console.WriteLine($"{Model} is driving on 4 wheels.");
            else
                Console.WriteLine($"Cannot drive {Model}. Engine is off.");
        }

        protected override void PlayEngineSound()
        {
            Console.WriteLine("Vroom vroom!");
        }
    }

    // Another Concrete Class
    public class Motorcycle : AbstractVehicle
    {
        public Motorcycle(string model) : base(model)
        {
        }

        public override void Move()
        {
            if (IsEngineRunning)
                Console.WriteLine($"{Model} is riding on 2 wheels.");
            else
                Console.WriteLine($"Cannot ride {Model}. Engine is off.");
        }

        protected override void PlayEngineSound()
        {
            Console.WriteLine("Braaap braaap!");
        }
    }
}
