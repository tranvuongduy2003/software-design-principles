using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.SOLID
{
    /// <summary>
    /// Open/Closed Principle (OCP)
    /// Software entities should be open for extension, but closed for modification.
    /// </summary>
    
    #region BAD Example
    public class BadShape
    {
        public string Type { get; set; } = string.Empty;
        public double Radius { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class BadAreaCalculator
    {
        public double CalculateTotalArea(IEnumerable<BadShape> shapes)
        {
            double area = 0;
            foreach (var shape in shapes)
            {
                if (shape.Type == "Circle")
                {
                    area += Math.PI * shape.Radius * shape.Radius;
                }
                else if (shape.Type == "Rectangle")
                {
                    area += shape.Width * shape.Height;
                }
                // Adding a new shape requires modifying this class!
            }
            return area;
        }
    }
    #endregion

    #region GOOD Example
    public interface IShape
    {
        double CalculateArea();
    }

    public class Circle : IShape
    {
        public double Radius { get; set; }
        public double CalculateArea() => Math.PI * Radius * Radius;
    }

    public class Rectangle : IShape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double CalculateArea() => Width * Height;
    }

    public class Triangle : IShape
    {
        public double Base { get; set; }
        public double Height { get; set; }
        public double CalculateArea() => (Base * Height) / 2;
    }

    public class AreaCalculator
    {
        public double CalculateTotalArea(IEnumerable<IShape> shapes)
        {
            double area = 0;
            foreach (var shape in shapes)
            {
                area += shape.CalculateArea();
            }
            return area;
        }
    }
    #endregion

    public static class OpenClosedDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Open/Closed Principle (OCP) ===");
            Console.WriteLine("Entities should be open for extension but closed for modification.\n");

            Console.WriteLine("--- BAD APPROACH ---");
            var badShapes = new List<BadShape>
            {
                new BadShape { Type = "Circle", Radius = 5 },
                new BadShape { Type = "Rectangle", Width = 4, Height = 5 }
            };
            var badCalc = new BadAreaCalculator();
            Console.WriteLine($"[Bad] Total Area: {badCalc.CalculateTotalArea(badShapes):F2}");
            Console.WriteLine("Explanation: To add a Triangle, we would have to modify the BadAreaCalculator class with another if-else statement.\n");

            Console.WriteLine("--- GOOD APPROACH ---");
            var goodShapes = new List<IShape>
            {
                new Circle { Radius = 5 },
                new Rectangle { Width = 4, Height = 5 },
                new Triangle { Base = 4, Height = 5 } // New shape added without changing AreaCalculator!
            };
            var calc = new AreaCalculator();
            Console.WriteLine($"[Good] Total Area: {calc.CalculateTotalArea(goodShapes):F2}");
            Console.WriteLine("Explanation: We added Triangle without modifying AreaCalculator. We extended functionality (open for extension) without changing existing code (closed for modification).");
            Console.WriteLine("===============================================\n");
        }
    }
}
