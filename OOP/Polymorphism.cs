using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.OOP
{
    /// <summary>
    /// Polymorphism means "many forms". It allows objects of different types to be treated as objects of a common base type.
    /// Contains compile-time (overloading) and runtime (overriding) polymorphism.
    /// </summary>
    public static class PolymorphismDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Polymorphism Demo ---");

            // Compile-time polymorphism (Method Overloading)
            Console.WriteLine("Compile-time Polymorphism (Overloading):");
            MathOperations math = new MathOperations();
            Console.WriteLine($"Add(int, int): {math.Add(5, 10)}");
            Console.WriteLine($"Add(double, double): {math.Add(5.5, 10.5)}");
            Console.WriteLine($"Add(int, int, int): {math.Add(5, 10, 15)}");
            Console.WriteLine();

            // Runtime polymorphism (Method Overriding)
            Console.WriteLine("Runtime Polymorphism (Overriding):");
            
            // Treating different shapes through the base class reference
            List<Shape> shapes = new List<Shape>
            {
                new Circle(5),
                new Rectangle(4, 6),
                new Triangle(4, 5)
            };

            foreach (var shape in shapes)
            {
                // Dynamic dispatch - calls the appropriate derived class method at runtime
                Console.WriteLine($"Shape: {shape.Name}, Area: {shape.CalculateArea():F2}");

                // Using 'is' and 'as' operators
                if (shape is Circle c)
                {
                    Console.WriteLine($"  -> This is a circle with radius {c.Radius}");
                }
                
                Rectangle? r = shape as Rectangle;
                if (r != null)
                {
                    Console.WriteLine($"  -> This is a rectangle with width {r.Width} and height {r.Height}");
                }
            }
            Console.WriteLine();
        }
    }

    // Compile-time polymorphism example
    public class MathOperations
    {
        public int Add(int a, int b) => a + b;
        public double Add(double a, double b) => a + b;
        public int Add(int a, int b, int c) => a + b + c;
    }

    // Runtime polymorphism example
    public abstract class Shape
    {
        public string Name { get; protected set; }

        protected Shape(string name)
        {
            Name = name;
        }

        // Virtual method to be overridden
        public virtual double CalculateArea()
        {
            return 0;
        }
    }

    public class Circle : Shape
    {
        public double Radius { get; set; }

        public Circle(double radius) : base("Circle")
        {
            Radius = radius;
        }

        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }
    }

    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height) : base("Rectangle")
        {
            Width = width;
            Height = height;
        }

        public override double CalculateArea()
        {
            return Width * Height;
        }
    }

    public class Triangle : Shape
    {
        public double BaseLength { get; set; }
        public double Height { get; set; }

        public Triangle(double baseLength, double height) : base("Triangle")
        {
            BaseLength = baseLength;
            Height = height;
        }

        public override double CalculateArea()
        {
            return 0.5 * BaseLength * Height;
        }
    }
}
