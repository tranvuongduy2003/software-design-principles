using System;

namespace SoftwareDesignPrinciples.SOLID
{
    /// <summary>
    /// Dependency Inversion Principle (DIP)
    /// High-level modules should not depend on low-level modules. Both should depend on abstractions.
    /// Abstractions should not depend on details. Details should depend on abstractions.
    /// </summary>
    
    #region BAD Example
    public class BadEmailSender
    {
        public void SendEmail(string message)
        {
            Console.WriteLine($"[Bad] Sending Email: {message}");
        }
    }

    public class BadNotificationService
    {
        private readonly BadEmailSender _emailSender;

        public BadNotificationService()
        {
            // Tightly coupled! The high-level module creates the low-level module directly.
            _emailSender = new BadEmailSender();
        }

        public void Notify(string message)
        {
            _emailSender.SendEmail(message);
        }
    }
    #endregion

    #region GOOD Example
    public interface IMessageSender
    {
        void SendMessage(string message);
    }

    public class EmailSender : IMessageSender
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"[Good] Sending Email: {message}");
        }
    }

    public class SmsSender : IMessageSender
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"[Good] Sending SMS: {message}");
        }
    }

    public class NotificationService
    {
        private readonly IMessageSender _messageSender;

        // Constructor Injection - depending on abstraction!
        public NotificationService(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public void Notify(string message)
        {
            _messageSender.SendMessage(message);
        }
    }
    #endregion

    public static class DependencyInversionDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Dependency Inversion Principle (DIP) ===");
            Console.WriteLine("Depend on abstractions, not on concretions.\n");

            Console.WriteLine("--- BAD APPROACH ---");
            var badService = new BadNotificationService();
            badService.Notify("Hello via Bad Service");
            Console.WriteLine("Explanation: BadNotificationService directly instantiates BadEmailSender. It's tightly coupled. If we want to send SMS, we must modify the service.\n");

            Console.WriteLine("--- GOOD APPROACH ---");
            // We can inject any IMessageSender implementation
            IMessageSender emailSender = new EmailSender();
            var serviceWithEmail = new NotificationService(emailSender);
            serviceWithEmail.Notify("Hello via Email");

            IMessageSender smsSender = new SmsSender();
            var serviceWithSms = new NotificationService(smsSender);
            serviceWithSms.Notify("Hello via SMS");
            
            Console.WriteLine("Explanation: NotificationService depends on IMessageSender interface. We can easily swap EmailSender with SmsSender or PushSender without changing NotificationService code.");
            Console.WriteLine("===============================================\n");
        }
    }
}
