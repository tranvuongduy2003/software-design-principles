using System;

namespace SoftwareDesignPrinciples.DesignPatterns.StructuralPatterns
{
    /// <summary>
    /// Facade Pattern
    /// Provides a unified interface to a set of interfaces in a subsystem.
    /// Facade defines a higher-level interface that makes the subsystem easier to use.
    /// </summary>
    public static class FacadePatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Facade Pattern Demo ---");
            Console.WriteLine("Scenario: Simplifying home theater operations.\n");

            var dvd = new DvdPlayer();
            var projector = new Projector();
            var sound = new SoundSystem();
            var lights = new TheaterLights();

            var homeTheater = new HomeTheaterFacade(dvd, projector, sound, lights);

            Console.WriteLine("Action: Client wants to watch a movie using Facade:");
            homeTheater.WatchMovie("Inception");

            Console.WriteLine("\nAction: Client wants to end the movie using Facade:");
            homeTheater.EndMovie();
            
            Console.WriteLine();
        }
    }

    // Subsystem Classes
    public class DvdPlayer
    {
        public void On() => Console.WriteLine("DVD Player is ON");
        public void Play(string movie) => Console.WriteLine($"DVD Player playing '{movie}'");
        public void Off() => Console.WriteLine("DVD Player is OFF");
    }

    public class Projector
    {
        public void On() => Console.WriteLine("Projector is ON");
        public void SetInput(DvdPlayer dvd) => Console.WriteLine("Projector input set to DVD Player");
        public void Off() => Console.WriteLine("Projector is OFF");
    }

    public class SoundSystem
    {
        public void On() => Console.WriteLine("Sound System is ON");
        public void SetVolume(int level) => Console.WriteLine($"Sound System volume set to {level}");
        public void Off() => Console.WriteLine("Sound System is OFF");
    }

    public class TheaterLights
    {
        public void Dim(int level) => Console.WriteLine($"Theater Lights dimmed to {level}%");
        public void On() => Console.WriteLine("Theater Lights are ON");
    }

    // Facade
    public class HomeTheaterFacade
    {
        private DvdPlayer _dvd;
        private Projector _projector;
        private SoundSystem _sound;
        private TheaterLights _lights;

        public HomeTheaterFacade(DvdPlayer dvd, Projector projector, SoundSystem sound, TheaterLights lights)
        {
            _dvd = dvd;
            _projector = projector;
            _sound = sound;
            _lights = lights;
        }

        public void WatchMovie(string movie)
        {
            Console.WriteLine("Get ready to watch a movie...");
            _lights.Dim(10);
            _projector.On();
            _projector.SetInput(_dvd);
            _sound.On();
            _sound.SetVolume(5);
            _dvd.On();
            _dvd.Play(movie);
        }

        public void EndMovie()
        {
            Console.WriteLine("Shutting movie theater down...");
            _lights.On();
            _sound.Off();
            _projector.Off();
            _dvd.Off();
        }
    }
}
