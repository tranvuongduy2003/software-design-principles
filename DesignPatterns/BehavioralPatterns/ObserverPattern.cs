using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.DesignPatterns.BehavioralPatterns
{
    /// <summary>
    /// Observer Pattern: Define a one-to-many dependency between objects so that when one object changes state,
    /// all its dependents are notified and updated automatically.
    /// </summary>
    public interface IObserver
    {
        void Update(string stockSymbol, decimal price);
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    public class StockTicker : ISubject
    {
        private readonly List<IObserver> _observers = new List<IObserver>();
        private string _symbol;
        private decimal _price;

        public StockTicker(string symbol)
        {
            _symbol = symbol;
        }

        public void SetPrice(decimal price)
        {
            _price = price;
            Console.WriteLine($"\n[Subject] {_symbol} price updated to {price:C}");
            Notify();
        }

        public void Attach(IObserver observer) => _observers.Add(observer);
        public void Detach(IObserver observer) => _observers.Remove(observer);

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_symbol, _price);
            }
        }
    }

    public class PriceDisplay : IObserver
    {
        public void Update(string stockSymbol, decimal price)
        {
            Console.WriteLine($"PriceDisplay: The current price for {stockSymbol} is {price:C}");
        }
    }

    public class PriceAlert : IObserver
    {
        private readonly decimal _threshold;

        public PriceAlert(decimal threshold)
        {
            _threshold = threshold;
        }

        public void Update(string stockSymbol, decimal price)
        {
            if (price > _threshold)
            {
                Console.WriteLine($"PriceAlert: ALERT! {stockSymbol} has exceeded threshold {_threshold:C}! Current: {price:C}");
            }
        }
    }

    public static class ObserverPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Observer Pattern Demo ---");
            Console.WriteLine("Theme: Stock price ticker");

            var aaplTicker = new StockTicker("AAPL");

            var display = new PriceDisplay();
            var alert = new PriceAlert(150.00m);

            aaplTicker.Attach(display);
            aaplTicker.Attach(alert);

            aaplTicker.SetPrice(145.00m);
            aaplTicker.SetPrice(149.50m);
            aaplTicker.SetPrice(152.00m);

            Console.WriteLine("-----------------------------\n");
        }
    }
}
