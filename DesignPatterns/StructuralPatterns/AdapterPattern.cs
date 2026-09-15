using System;
using System.Text.Json;
using System.Xml.Linq;

namespace SoftwareDesignPrinciples.DesignPatterns.StructuralPatterns
{
    /// <summary>
    /// Adapter Pattern
    /// Converts the interface of a class into another interface clients expect.
    /// Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.
    /// </summary>
    public static class AdapterPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Adapter Pattern Demo ---");
            Console.WriteLine("Scenario: Integrating an incompatible XML analytics library into a system expecting JSON.\n");

            // 1. The client works with JSON
            string jsonInput = "{\"User\": \"Alice\", \"Action\": \"Login\"}";
            IJsonAnalytics clientAnalytics = new JsonToXmlAdapter(new XmlAnalyticsLibrary());
            
            Console.WriteLine("Client sending JSON data...");
            clientAnalytics.AnalyzeData(jsonInput);
            
            Console.WriteLine();
        }
    }

    // Target interface expected by the client
    public interface IJsonAnalytics
    {
        void AnalyzeData(string jsonInput);
    }

    // Adaptee: The incompatible library that only understands XML
    public class XmlAnalyticsLibrary
    {
        public void ProcessXml(string xmlInput)
        {
            Console.WriteLine("XmlAnalyticsLibrary: Processing XML data:");
            Console.WriteLine(xmlInput);
        }
    }

    // Adapter: Wraps the Adaptee and implements the Target interface
    public class JsonToXmlAdapter : IJsonAnalytics
    {
        private readonly XmlAnalyticsLibrary _xmlLibrary;

        public JsonToXmlAdapter(XmlAnalyticsLibrary xmlLibrary)
        {
            _xmlLibrary = xmlLibrary;
        }

        public void AnalyzeData(string jsonInput)
        {
            Console.WriteLine("JsonToXmlAdapter: Converting JSON to XML...");
            
            // Very simple mock conversion
            string xmlOutput = "<Data><User>Alice</User><Action>Login</Action></Data>";
            
            _xmlLibrary.ProcessXml(xmlOutput);
        }
    }
}
