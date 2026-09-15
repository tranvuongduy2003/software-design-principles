using System;

namespace SoftwareDesignPrinciples.DesignPatterns.BehavioralPatterns
{
    /// <summary>
    /// State Pattern: Allow an object to alter its behavior when its internal state changes.
    /// The object will appear to change its class.
    /// </summary>
    public interface IVendingMachineState
    {
        void InsertCoin();
        void SelectItem();
        void DispenseItem();
    }

    public class VendingMachine
    {
        public IVendingMachineState IdleState { get; }
        public IVendingMachineState HasMoneyState { get; }
        public IVendingMachineState DispensingState { get; }
        public IVendingMachineState OutOfStockState { get; }

        public IVendingMachineState CurrentState { get; set; }
        public int StockCount { get; set; }

        public VendingMachine(int initialStock)
        {
            IdleState = new IdleState(this);
            HasMoneyState = new HasMoneyState(this);
            DispensingState = new DispensingState(this);
            OutOfStockState = new OutOfStockState(this);

            StockCount = initialStock;
            CurrentState = StockCount > 0 ? IdleState : OutOfStockState;
        }

        public void InsertCoin() => CurrentState.InsertCoin();
        public void SelectItem() => CurrentState.SelectItem();
        public void DispenseItem() => CurrentState.DispenseItem();
    }

    public class IdleState : IVendingMachineState
    {
        private readonly VendingMachine _machine;
        public IdleState(VendingMachine machine) => _machine = machine;

        public void InsertCoin()
        {
            Console.WriteLine("Coin inserted.");
            _machine.CurrentState = _machine.HasMoneyState;
        }
        public void SelectItem() => Console.WriteLine("Please insert a coin first.");
        public void DispenseItem() => Console.WriteLine("No coin inserted.");
    }

    public class HasMoneyState : IVendingMachineState
    {
        private readonly VendingMachine _machine;
        public HasMoneyState(VendingMachine machine) => _machine = machine;

        public void InsertCoin() => Console.WriteLine("Coin already inserted.");
        public void SelectItem()
        {
            Console.WriteLine("Item selected.");
            _machine.CurrentState = _machine.DispensingState;
        }
        public void DispenseItem() => Console.WriteLine("Select an item first.");
    }

    public class DispensingState : IVendingMachineState
    {
        private readonly VendingMachine _machine;
        public DispensingState(VendingMachine machine) => _machine = machine;

        public void InsertCoin() => Console.WriteLine("Please wait, dispensing item.");
        public void SelectItem() => Console.WriteLine("Already dispensing.");
        public void DispenseItem()
        {
            Console.WriteLine("Dispensing item...");
            _machine.StockCount--;
            if (_machine.StockCount > 0)
            {
                _machine.CurrentState = _machine.IdleState;
            }
            else
            {
                Console.WriteLine("Machine is now out of stock.");
                _machine.CurrentState = _machine.OutOfStockState;
            }
        }
    }

    public class OutOfStockState : IVendingMachineState
    {
        private readonly VendingMachine _machine;
        public OutOfStockState(VendingMachine machine) => _machine = machine;

        public void InsertCoin() => Console.WriteLine("Cannot accept coin. Out of stock.");
        public void SelectItem() => Console.WriteLine("Out of stock.");
        public void DispenseItem() => Console.WriteLine("Out of stock.");
    }

    public static class StatePatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- State Pattern Demo ---");
            Console.WriteLine("Theme: Vending machine states");

            var machine = new VendingMachine(2);

            Console.WriteLine("\n[Buying first item]");
            machine.InsertCoin();
            machine.SelectItem();
            machine.DispenseItem();

            Console.WriteLine("\n[Buying second item]");
            machine.InsertCoin();
            machine.SelectItem();
            machine.DispenseItem();

            Console.WriteLine("\n[Trying to buy third item]");
            machine.InsertCoin();

            Console.WriteLine("--------------------------\n");
        }
    }
}
