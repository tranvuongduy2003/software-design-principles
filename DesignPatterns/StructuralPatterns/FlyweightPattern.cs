using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.DesignPatterns.StructuralPatterns
{
    /// <summary>
    /// Flyweight Pattern
    /// Uses sharing to support large numbers of fine-grained objects efficiently.
    /// </summary>
    public static class FlyweightPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Flyweight Pattern Demo ---");
            Console.WriteLine("Scenario: Text editor sharing character formatting styles.\n");

            var factory = new CharacterStyleFactory();
            var text = new List<Character>();

            // Creating text "HELLO" where some letters share styles
            text.Add(new Character('H', factory.GetStyle("Arial", 12, "Black")));
            text.Add(new Character('E', factory.GetStyle("Arial", 12, "Black")));
            text.Add(new Character('L', factory.GetStyle("Arial", 14, "Red")));
            text.Add(new Character('L', factory.GetStyle("Arial", 14, "Red")));
            text.Add(new Character('O', factory.GetStyle("Arial", 12, "Black")));

            foreach (var c in text)
            {
                c.Display();
            }

            Console.WriteLine($"\nTotal Characters Created: {text.Count}");
            Console.WriteLine($"Total Shared Styles (Flyweights) Created: {factory.GetStyleCount()}");
            Console.WriteLine("Savings: 5 characters share only 2 distinct styles!");
            
            Console.WriteLine();
        }
    }

    // Flyweight
    public class CharacterStyle
    {
        public string Font { get; }
        public int Size { get; }
        public string Color { get; }

        public CharacterStyle(string font, int size, string color)
        {
            Font = font;
            Size = size;
            Color = color;
            Console.WriteLine($"[Flyweight] Created new style: {font}, {size}pt, {color}");
        }
    }

    // Flyweight Factory
    public class CharacterStyleFactory
    {
        private Dictionary<string, CharacterStyle> _styles = new Dictionary<string, CharacterStyle>();

        public CharacterStyle GetStyle(string font, int size, string color)
        {
            string key = $"{font}_{size}_{color}";
            if (!_styles.ContainsKey(key))
            {
                _styles[key] = new CharacterStyle(font, size, color);
            }
            return _styles[key];
        }

        public int GetStyleCount() => _styles.Count;
    }

    // Context (contains extrinsic state)
    public class Character
    {
        private char _symbol;
        private CharacterStyle _style; // Reference to flyweight

        public Character(char symbol, CharacterStyle style)
        {
            _symbol = symbol;
            _style = style;
        }

        public void Display()
        {
            Console.WriteLine($"Character: '{_symbol}' [Style: {_style.Font}, {_style.Size}pt, {_style.Color}]");
        }
    }
}
