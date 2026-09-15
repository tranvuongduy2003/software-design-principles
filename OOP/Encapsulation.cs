using System;

namespace SoftwareDesignPrinciples.OOP
{
    /// <summary>
    /// Encapsulation is the bundling of data (fields) and methods that operate on that data within a single unit (class).
    /// It restricts direct access to some of the object's components, which is a means of preventing accidental interference and misuse.
    /// </summary>
    public static class EncapsulationDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Encapsulation Demo ---");

            // BAD Example
            Console.WriteLine("BAD EXAMPLE: Public fields");
            BadBankAccount badAccount = new BadBankAccount();
            badAccount.Balance = 100; // Direct access
            badAccount.Balance = -500; // No validation!
            Console.WriteLine($"Bad Account Balance: {badAccount.Balance}");
            
            Console.WriteLine("\nGOOD EXAMPLE: Encapsulation with properties and methods");
            BankAccount goodAccount = new BankAccount("John Doe", 1000m);
            Console.WriteLine($"Initial Balance: {goodAccount.Balance}");
            
            goodAccount.Deposit(500);
            Console.WriteLine($"After Deposit: {goodAccount.Balance}");
            
            goodAccount.Withdraw(200);
            Console.WriteLine($"After Withdraw: {goodAccount.Balance}");
            
            try
            {
                goodAccount.Withdraw(5000); // Should fail
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            Console.WriteLine();
        }
    }

    // BAD: No encapsulation
    public class BadBankAccount
    {
        public decimal Balance; // Public field, no validation
    }

    // GOOD: Encapsulated
    public class BankAccount
    {
        // Private fields
        private decimal _balance;
        
        // Auto-property with private setter (read-only externally)
        public string OwnerName { get; private set; }
        
        // Full property with controlled access
        public decimal Balance
        {
            get { return _balance; }
            private set { _balance = value; } // Only class can change balance directly
        }

        public BankAccount(string ownerName, decimal initialBalance)
        {
            OwnerName = ownerName;
            if (initialBalance >= 0)
            {
                _balance = initialBalance;
            }
        }

        // Public methods providing controlled ways to modify internal state
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive.");
            }
            _balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be positive.");
            }
            if (amount > _balance)
            {
                throw new ArgumentException("Insufficient funds.");
            }
            _balance -= amount;
        }
    }
}
