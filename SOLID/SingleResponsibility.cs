using System;

namespace SoftwareDesignPrinciples.SOLID
{
    /// <summary>
    /// Single Responsibility Principle (SRP)
    /// A class should have one, and only one, reason to change.
    /// </summary>
    
    #region BAD Example
    public class BadInvoice
    {
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }

        public decimal CalculateTotal()
        {
            return Amount + (Amount * TaxRate);
        }

        public void PrintInvoice()
        {
            Console.WriteLine($"[Bad] Invoice Total: {CalculateTotal()}");
        }

        public void SaveToDatabase()
        {
            Console.WriteLine("[Bad] Saving invoice to database...");
        }
    }
    #endregion

    #region GOOD Example
    public class Invoice
    {
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }

        public decimal CalculateTotal()
        {
            return Amount + (Amount * TaxRate);
        }
    }

    public class InvoicePrinter
    {
        public void Print(Invoice invoice)
        {
            Console.WriteLine($"[Good] Formatting and printing invoice. Total: {invoice.CalculateTotal():C}");
        }
    }

    public class InvoiceRepository
    {
        public void Save(Invoice invoice)
        {
            Console.WriteLine($"[Good] Saving invoice of {invoice.CalculateTotal():C} to database securely...");
        }
    }
    #endregion

    public static class SingleResponsibilityDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== Single Responsibility Principle (SRP) ===");
            Console.WriteLine("A class should have only one reason to change.\n");

            Console.WriteLine("--- BAD APPROACH ---");
            var badInvoice = new BadInvoice { Amount = 100, TaxRate = 0.10m };
            badInvoice.PrintInvoice();
            badInvoice.SaveToDatabase();
            Console.WriteLine("Explanation: The BadInvoice class handles business logic, formatting/printing, AND database persistence. If printing logic changes, the Invoice class must change.\n");

            Console.WriteLine("--- GOOD APPROACH ---");
            var invoice = new Invoice { Amount = 100, TaxRate = 0.10m };
            
            var printer = new InvoicePrinter();
            printer.Print(invoice);

            var repository = new InvoiceRepository();
            repository.Save(invoice);

            Console.WriteLine("Explanation: We separated the concerns. 'Invoice' handles data/logic, 'InvoicePrinter' handles display, and 'InvoiceRepository' handles persistence.");
            Console.WriteLine("===============================================\n");
        }
    }
}
