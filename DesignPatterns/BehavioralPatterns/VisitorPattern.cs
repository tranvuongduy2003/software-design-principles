using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.DesignPatterns.BehavioralPatterns
{
    /// <summary>
    /// Visitor Pattern: Represent an operation to be performed on the elements of an object structure.
    /// Visitor lets you define a new operation without changing the classes of the elements on which it operates.
    /// </summary>
    public interface IVisitor
    {
        void Visit(BookElement book);
        void Visit(FruitElement fruit);
        void Visit(ElectronicsElement electronics);
    }

    public interface IShoppingElement
    {
        void Accept(IVisitor visitor);
    }

    public class BookElement : IShoppingElement
    {
        public decimal Price { get; }
        public decimal Weight { get; }

        public BookElement(decimal price, decimal weight)
        {
            Price = price;
            Weight = weight;
        }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class FruitElement : IShoppingElement
    {
        public decimal PricePerKg { get; }
        public decimal Weight { get; }

        public FruitElement(decimal pricePerKg, decimal weight)
        {
            PricePerKg = pricePerKg;
            Weight = weight;
        }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class ElectronicsElement : IShoppingElement
    {
        public decimal Price { get; }
        public bool IsFragile { get; }

        public ElectronicsElement(decimal price, bool isFragile)
        {
            Price = price;
            IsFragile = isFragile;
        }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class PricingVisitor : IVisitor
    {
        public decimal TotalPrice { get; private set; }

        public void Visit(BookElement book)
        {
            // Shipping cost based on weight, no tax
            var cost = book.Price + (book.Weight * 2m);
            Console.WriteLine($"Book: Base {book.Price:C} + Shipping -> {cost:C}");
            TotalPrice += cost;
        }

        public void Visit(FruitElement fruit)
        {
            // Price by weight, perishable shipping cost
            var cost = (fruit.PricePerKg * fruit.Weight) + 5m;
            Console.WriteLine($"Fruit: {fruit.Weight}kg at {fruit.PricePerKg:C} + Cold Shipping -> {cost:C}");
            TotalPrice += cost;
        }

        public void Visit(ElectronicsElement electronics)
        {
            // Tax and fragile shipping
            var cost = electronics.Price * 1.10m + (electronics.IsFragile ? 15m : 5m);
            Console.WriteLine($"Electronics: Base {electronics.Price:C} + Tax + Shipping -> {cost:C}");
            TotalPrice += cost;
        }
    }

    public class XmlExportVisitor : IVisitor
    {
        public string XmlData { get; private set; } = "<cart>\n";

        public void Visit(BookElement book)
        {
            XmlData += $"  <item type='book' price='{book.Price}' weight='{book.Weight}' />\n";
        }

        public void Visit(FruitElement fruit)
        {
            XmlData += $"  <item type='fruit' pricePerKg='{fruit.PricePerKg}' weight='{fruit.Weight}' />\n";
        }

        public void Visit(ElectronicsElement electronics)
        {
            XmlData += $"  <item type='electronics' price='{electronics.Price}' isFragile='{electronics.IsFragile}' />\n";
        }

        public void Finish() => XmlData += "</cart>";
    }

    public static class VisitorPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Visitor Pattern Demo ---");
            Console.WriteLine("Theme: Shopping cart elements and operations");

            var cart = new List<IShoppingElement>
            {
                new BookElement(20m, 1.5m),
                new FruitElement(3.5m, 2m),
                new ElectronicsElement(500m, true)
            };

            Console.WriteLine("\n[Pricing Visitor]");
            var pricingVisitor = new PricingVisitor();
            foreach (var item in cart)
            {
                item.Accept(pricingVisitor);
            }
            Console.WriteLine($"Total Cart Price: {pricingVisitor.TotalPrice:C}");

            Console.WriteLine("\n[XML Export Visitor]");
            var xmlVisitor = new XmlExportVisitor();
            foreach (var item in cart)
            {
                item.Accept(xmlVisitor);
            }
            xmlVisitor.Finish();
            Console.WriteLine(xmlVisitor.XmlData);

            Console.WriteLine("----------------------------\n");
        }
    }
}
