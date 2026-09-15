using System;

namespace SoftwareDesignPrinciples.DesignPatterns.BehavioralPatterns
{
    /// <summary>
    /// Chain of Responsibility Pattern: Allows an object to send a command without knowing what object will receive and handle it.
    /// The request is sent from one object to another making them a chain.
    /// </summary>
    public abstract class SupportHandler
    {
        protected SupportHandler? _nextHandler;

        public void SetNext(SupportHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public abstract void Handle(int severity);
    }

    public class Level1Support : SupportHandler
    {
        public override void Handle(int severity)
        {
            if (severity <= 1)
            {
                Console.WriteLine("Level 1 Support: Handled the basic issue.");
            }
            else if (_nextHandler != null)
            {
                Console.WriteLine("Level 1 Support: Issue too complex, escalating...");
                _nextHandler.Handle(severity);
            }
        }
    }

    public class Level2Support : SupportHandler
    {
        public override void Handle(int severity)
        {
            if (severity <= 2)
            {
                Console.WriteLine("Level 2 Support: Handled the intermediate issue.");
            }
            else if (_nextHandler != null)
            {
                Console.WriteLine("Level 2 Support: Issue too complex, escalating...");
                _nextHandler.Handle(severity);
            }
        }
    }

    public class Level3Support : SupportHandler
    {
        public override void Handle(int severity)
        {
            if (severity <= 3)
            {
                Console.WriteLine("Level 3 Support: Handled the advanced issue.");
            }
            else if (_nextHandler != null)
            {
                Console.WriteLine("Level 3 Support: Issue requires management attention, escalating...");
                _nextHandler.Handle(severity);
            }
        }
    }

    public class ManagerSupport : SupportHandler
    {
        public override void Handle(int severity)
        {
            Console.WriteLine($"Manager: Handling critical issue (Severity {severity}) personally.");
        }
    }

    public static class ChainOfResponsibilityPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Chain of Responsibility Pattern Demo ---");
            Console.WriteLine("Theme: Support ticket escalation");
            
            var level1 = new Level1Support();
            var level2 = new Level2Support();
            var level3 = new Level3Support();
            var manager = new ManagerSupport();

            level1.SetNext(level2);
            level2.SetNext(level3);
            level3.SetNext(manager);

            Console.WriteLine("\nTicket Severity 1:");
            level1.Handle(1);

            Console.WriteLine("\nTicket Severity 2:");
            level1.Handle(2);

            Console.WriteLine("\nTicket Severity 3:");
            level1.Handle(3);

            Console.WriteLine("\nTicket Severity 5 (Critical):");
            level1.Handle(5);
            
            Console.WriteLine("--------------------------------------------\n");
        }
    }
}
