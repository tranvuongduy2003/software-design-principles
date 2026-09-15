using System;

namespace SoftwareDesignPrinciples.DesignPatterns.BehavioralPatterns
{
    /// <summary>
    /// Template Method Pattern: Define the skeleton of an algorithm in an operation, deferring some steps to subclasses.
    /// Template Method lets subclasses redefine certain steps of an algorithm without changing the algorithm's structure.
    /// </summary>
    public abstract class DataMiner
    {
        // The template method
        public void Mine(string path)
        {
            OpenFile(path);
            ExtractData();
            ParseData();
            AnalyzeData();
            CloseFile();
        }

        protected void OpenFile(string path) => Console.WriteLine($"Opening file: {path}");
        
        protected abstract void ExtractData();
        protected abstract void ParseData();
        
        protected virtual void AnalyzeData()
        {
            Console.WriteLine("Analyzing data using default common analysis algorithms.");
        }
        
        protected void CloseFile() => Console.WriteLine("Closing file.");
    }

    public class CsvDataMiner : DataMiner
    {
        protected override void ExtractData()
        {
            Console.WriteLine("Extracting data from CSV format.");
        }

        protected override void ParseData()
        {
            Console.WriteLine("Parsing CSV rows and columns.");
        }
    }

    public class JsonDataMiner : DataMiner
    {
        protected override void ExtractData()
        {
            Console.WriteLine("Extracting data from JSON format.");
        }

        protected override void ParseData()
        {
            Console.WriteLine("Parsing JSON objects and arrays.");
        }
    }

    public class XmlDataMiner : DataMiner
    {
        protected override void ExtractData()
        {
            Console.WriteLine("Extracting data from XML format.");
        }

        protected override void ParseData()
        {
            Console.WriteLine("Parsing XML nodes and attributes.");
        }

        // Overriding the hook
        protected override void AnalyzeData()
        {
            Console.WriteLine("Analyzing XML data using custom XML-specific algorithms.");
        }
    }

    public static class TemplateMethodPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Template Method Pattern Demo ---");
            Console.WriteLine("Theme: Data mining file parser\n");

            Console.WriteLine("Processing CSV:");
            DataMiner csvMiner = new CsvDataMiner();
            csvMiner.Mine("data.csv");

            Console.WriteLine("\nProcessing JSON:");
            DataMiner jsonMiner = new JsonDataMiner();
            jsonMiner.Mine("data.json");

            Console.WriteLine("\nProcessing XML:");
            DataMiner xmlMiner = new XmlDataMiner();
            xmlMiner.Mine("data.xml");

            Console.WriteLine("------------------------------------\n");
        }
    }
}
