using System;

namespace SoftwareDesignPrinciples.DesignPatterns.BehavioralPatterns
{
    /// <summary>
    /// Strategy Pattern: Define a family of algorithms, encapsulate each one, and make them interchangeable.
    /// Strategy lets the algorithm vary independently from clients that use it.
    /// </summary>
    public interface IPaymentStrategy
    {
        void Pay(decimal amount);
    }

    public class CreditCardPayment : IPaymentStrategy
    {
        private readonly string _cardNumber;
        public CreditCardPayment(string cardNumber) => _cardNumber = cardNumber;

        public void Pay(decimal amount)
        {
            Console.WriteLine($"Paid {amount:C} using Credit Card ending in {_cardNumber.Substring(_cardNumber.Length - 4)}.");
        }
    }

    public class PayPalPayment : IPaymentStrategy
    {
        private readonly string _email;
        public PayPalPayment(string email) => _email = email;

        public void Pay(decimal amount)
        {
            Console.WriteLine($"Paid {amount:C} using PayPal account {_email}.");
        }
    }

    public class BitcoinPayment : IPaymentStrategy
    {
        private readonly string _walletAddress;
        public BitcoinPayment(string walletAddress) => _walletAddress = walletAddress;

        public void Pay(decimal amount)
        {
            Console.WriteLine($"Paid {amount:C} using Bitcoin to address {_walletAddress}.");
        }
    }

    public class ShoppingCart
    {
        public decimal TotalAmount { get; set; }

        public void Checkout(IPaymentStrategy paymentStrategy)
        {
            Console.WriteLine($"Checking out {TotalAmount:C}...");
            paymentStrategy.Pay(TotalAmount);
        }
    }

    public static class StrategyPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Strategy Pattern Demo ---");
            Console.WriteLine("Theme: Payment processing");

            var cart = new ShoppingCart { TotalAmount = 150.75m };

            Console.WriteLine("\nUsing Credit Card:");
            cart.Checkout(new CreditCardPayment("1234567890123456"));

            Console.WriteLine("\nUsing PayPal:");
            cart.Checkout(new PayPalPayment("user@example.com"));

            Console.WriteLine("\nUsing Bitcoin:");
            cart.Checkout(new BitcoinPayment("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa"));

            Console.WriteLine("-----------------------------\n");
        }
    }
}
