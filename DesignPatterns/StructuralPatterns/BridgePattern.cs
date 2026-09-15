using System;

namespace SoftwareDesignPrinciples.DesignPatterns.StructuralPatterns
{
    /// <summary>
    /// Bridge Pattern
    /// Decouples an abstraction from its implementation so that the two can vary independently.
    /// </summary>
    public static class BridgePatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Bridge Pattern Demo ---");
            Console.WriteLine("Scenario: Remote controls (Abstraction) and Devices (Implementation).\n");

            IDevice tv = new Tv();
            RemoteControl basicRemote = new RemoteControl(tv);
            basicRemote.TogglePower();

            Console.WriteLine();

            IDevice radio = new Radio();
            AdvancedRemote advancedRemote = new AdvancedRemote(radio);
            advancedRemote.TogglePower();
            advancedRemote.Mute();

            Console.WriteLine();
        }
    }

    // Implementor
    public interface IDevice
    {
        bool IsEnabled { get; set; }
        void Enable();
        void Disable();
        int Volume { get; set; }
    }

    // Concrete Implementor A
    public class Tv : IDevice
    {
        public bool IsEnabled { get; set; }
        public int Volume { get; set; } = 30;

        public void Enable()
        {
            IsEnabled = true;
            Console.WriteLine("TV is turned ON.");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("TV is turned OFF.");
        }
    }

    // Concrete Implementor B
    public class Radio : IDevice
    {
        public bool IsEnabled { get; set; }
        public int Volume { get; set; } = 20;

        public void Enable()
        {
            IsEnabled = true;
            Console.WriteLine("Radio is turned ON.");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("Radio is turned OFF.");
        }
    }

    // Abstraction
    public class RemoteControl
    {
        protected IDevice _device;

        public RemoteControl(IDevice device)
        {
            _device = device;
        }

        public virtual void TogglePower()
        {
            if (_device.IsEnabled)
            {
                _device.Disable();
            }
            else
            {
                _device.Enable();
            }
        }

        public virtual void VolumeDown()
        {
            _device.Volume -= 10;
            Console.WriteLine($"Volume set to {_device.Volume}");
        }

        public virtual void VolumeUp()
        {
            _device.Volume += 10;
            Console.WriteLine($"Volume set to {_device.Volume}");
        }
    }

    // Refined Abstraction
    public class AdvancedRemote : RemoteControl
    {
        public AdvancedRemote(IDevice device) : base(device) { }

        public void Mute()
        {
            _device.Volume = 0;
            Console.WriteLine("Device is MUTED.");
        }
    }
}
