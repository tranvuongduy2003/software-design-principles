using System;

namespace SoftwareDesignPrinciples.DesignPatterns.CreationalPatterns;

/// <summary>
/// Factory Method Pattern
/// Defines an interface for creating an object, but lets subclasses decide which class to instantiate.
/// </summary>
public static class FactoryMethodPatternDemo
{
    public static void Run()
    {
        Console.WriteLine("\n--- Factory Method Pattern Demo ---");

        DocumentCreator pdfCreator = new PdfDocumentCreator();
        DocumentCreator wordCreator = new WordDocumentCreator();
        DocumentCreator excelCreator = new ExcelDocumentCreator();

        var doc1 = pdfCreator.CreateDocument();
        var doc2 = wordCreator.CreateDocument();
        var doc3 = excelCreator.CreateDocument();

        doc1.Render();
        doc2.Render();
        doc3.Render();
    }
}

public interface IDocument
{
    void Render();
}

public class PdfDocument : IDocument
{
    public void Render() => Console.WriteLine("Rendering PDF Document...");
}

public class WordDocument : IDocument
{
    public void Render() => Console.WriteLine("Rendering Word Document...");
}

public class ExcelDocument : IDocument
{
    public void Render() => Console.WriteLine("Rendering Excel Document...");
}

public abstract class DocumentCreator
{
    public abstract IDocument CreateDocument();
}

public class PdfDocumentCreator : DocumentCreator
{
    public override IDocument CreateDocument() => new PdfDocument();
}

public class WordDocumentCreator : DocumentCreator
{
    public override IDocument CreateDocument() => new WordDocument();
}

public class ExcelDocumentCreator : DocumentCreator
{
    public override IDocument CreateDocument() => new ExcelDocument();
}
