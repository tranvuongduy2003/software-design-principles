using System;

namespace SoftwareDesignPrinciples.SOLID
{
    /// <summary>
    /// Liskov Substitution Principle (LSP)
    /// Objects of a superclass shall be replaceable with objects of its subclasses without breaking the application.
    /// </summary>
    
    #region BAD Example
    public class BadBird
    {
        public virtual void Fly()
        {
            Console.WriteLine("Bird is flying...");
        }
    }

    public class BadEagle : BadBird
    {
        public override void Fly()
        {
            Console.WriteLine("Eagle flies high!");
        }
    }

    public class BadPenguin : BadBird
    {
        public override void Fly()
        {
            throw new NotSupportedException("Penguins cannot fly!");
        }
    }
    #endregion

    #region GOOD Example
    public interface IBird
    {
        void Move();
    }

    public interface IFlyableBird : IBird
    {
        void Fly();
    }

    public class Eagle : IFlyableBird
    {
        public void Move()
        {
            Console.WriteLine("Eagle is hopping...");
        }

        public void Fly()
        {
            Console.WriteLine("Eagle flies high!");
        }
    }

    public class Penguin : IBird
    {
        public void Move()
        {
            Console.WriteLine("Penguin is swimming/waddling...");
        }
    }
    #endregion

    public static class LiskovSubstitutionDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Liskov Substitution Principle (LSP) ===");
            Console.WriteLine("Subtypes must be substitutable for their base types.\n");

            Console.WriteLine("--- BAD APPROACH ---");
            BadBird badBird = new BadEagle();
            badBird.Fly(); // Works fine

            BadBird anotherBadBird = new BadPenguin();
            try
            {
                anotherBadBird.Fly(); // Throws exception!
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Bad] Error: {ex.Message}");
            }
            Console.WriteLine("Explanation: Penguin violates LSP because it cannot substitute its base class Bird without causing an error.\n");

            Console.WriteLine("--- GOOD APPROACH ---");
            IFlyableBird eagle = new Eagle();
            eagle.Fly();

            IBird penguin = new Penguin();
            penguin.Move();
            
            Console.WriteLine("Explanation: Separating interfaces (IBird and IFlyableBird) ensures we don't force non-flying birds to implement a Fly method. Now subtypes are fully substitutable.");
            Console.WriteLine("===============================================\n");
        }
    }
}
