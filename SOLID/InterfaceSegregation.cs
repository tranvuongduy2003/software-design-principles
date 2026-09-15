using System;

namespace SoftwareDesignPrinciples.SOLID
{
    /// <summary>
    /// Interface Segregation Principle (ISP)
    /// No client should be forced to depend on methods it does not use.
    /// </summary>
    
    #region BAD Example
    public interface IBadWorker
    {
        void Work();
        void Eat();
        void Sleep();
    }

    public class BadHumanWorker : IBadWorker
    {
        public void Work() => Console.WriteLine("[Bad] Human working...");
        public void Eat() => Console.WriteLine("[Bad] Human eating lunch...");
        public void Sleep() => Console.WriteLine("[Bad] Human sleeping...");
    }

    public class BadRobotWorker : IBadWorker
    {
        public void Work() => Console.WriteLine("[Bad] Robot working non-stop...");
        
        public void Eat()
        {
            throw new NotSupportedException("Robots do not eat!");
        }

        public void Sleep()
        {
            throw new NotSupportedException("Robots do not sleep!");
        }
    }
    #endregion

    #region GOOD Example
    public interface IWorkable
    {
        void Work();
    }

    public interface IFeedable
    {
        void Eat();
    }

    public interface ISleepable
    {
        void Sleep();
    }

    public class HumanWorker : IWorkable, IFeedable, ISleepable
    {
        public void Work() => Console.WriteLine("[Good] Human working...");
        public void Eat() => Console.WriteLine("[Good] Human eating lunch...");
        public void Sleep() => Console.WriteLine("[Good] Human sleeping...");
    }

    public class RobotWorker : IWorkable
    {
        public void Work() => Console.WriteLine("[Good] Robot working non-stop...");
    }
    #endregion

    public static class InterfaceSegregationDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Interface Segregation Principle (ISP) ===");
            Console.WriteLine("Clients shouldn't be forced to depend on interfaces they don't use.\n");

            Console.WriteLine("--- BAD APPROACH ---");
            IBadWorker robot = new BadRobotWorker();
            robot.Work();
            try
            {
                robot.Eat();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Bad] Error: {ex.Message}");
            }
            Console.WriteLine("Explanation: Robot is forced to implement Eat() and Sleep() which it doesn't need. This violates ISP.\n");

            Console.WriteLine("--- GOOD APPROACH ---");
            IWorkable human = new HumanWorker();
            human.Work();
            if (human is IFeedable feedableHuman)
            {
                feedableHuman.Eat();
            }

            IWorkable goodRobot = new RobotWorker();
            goodRobot.Work();
            // goodRobot.Eat(); // Compiler error, which is correct! Robot doesn't implement IFeedable.
            
            Console.WriteLine("Explanation: By splitting the fat IWorker interface into smaller, specific interfaces (IWorkable, IFeedable, ISleepable), clients only implement what they actually need.");
            Console.WriteLine("===============================================\n");
        }
    }
}
