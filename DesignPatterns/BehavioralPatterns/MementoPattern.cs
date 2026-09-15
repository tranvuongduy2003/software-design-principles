using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.DesignPatterns.BehavioralPatterns
{
    /// <summary>
    /// Memento Pattern: Without violating encapsulation, capture and externalize an object's
    /// internal state so that the object can be restored to this state later.
    /// </summary>
    public class GameState
    {
        public int Health { get; }
        public int Level { get; }
        public string Position { get; }

        public GameState(int health, int level, string position)
        {
            Health = health;
            Level = level;
            Position = position;
        }
    }

    public class Player
    {
        public int Health { get; set; }
        public int Level { get; set; }
        public string Position { get; set; }

        public Player(int health, int level, string position)
        {
            Health = health;
            Level = level;
            Position = position;
        }

        public void PrintState()
        {
            Console.WriteLine($"Player State -> Health: {Health}, Level: {Level}, Position: {Position}");
        }

        public GameState Save()
        {
            Console.WriteLine("Saving game state...");
            return new GameState(Health, Level, Position);
        }

        public void Restore(GameState memento)
        {
            Console.WriteLine("Restoring game state...");
            Health = memento.Health;
            Level = memento.Level;
            Position = memento.Position;
        }
    }

    public class GameSaveManager
    {
        private readonly List<GameState> _saves = new List<GameState>();

        public void AddSave(GameState memento)
        {
            _saves.Add(memento);
        }

        public GameState GetSave(int index)
        {
            if (index >= 0 && index < _saves.Count)
            {
                return _saves[index];
            }
            throw new IndexOutOfRangeException("Invalid save index.");
        }
    }

    public static class MementoPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Memento Pattern Demo ---");
            Console.WriteLine("Theme: Game save system");

            var player = new Player(100, 1, "Start Village");
            var saveManager = new GameSaveManager();

            player.PrintState();
            saveManager.AddSave(player.Save()); // Save 0

            Console.WriteLine("\nTaking damage and leveling up...");
            player.Health = 50;
            player.Level = 2;
            player.Position = "Dark Forest";
            player.PrintState();
            saveManager.AddSave(player.Save()); // Save 1

            Console.WriteLine("\nDied! Restoring to first save...");
            player.Restore(saveManager.GetSave(0));
            player.PrintState();

            Console.WriteLine("----------------------------\n");
        }
    }
}
