using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.DesignPatterns.StructuralPatterns
{
    /// <summary>
    /// Composite Pattern
    /// Composes objects into tree structures to represent part-whole hierarchies.
    /// Composite lets clients treat individual objects and compositions of objects uniformly.
    /// </summary>
    public static class CompositePatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Composite Pattern Demo ---");
            Console.WriteLine("Scenario: File system hierarchy.\n");

            FolderItem root = new FolderItem("root");
            FolderItem docs = new FolderItem("Documents");
            FolderItem pics = new FolderItem("Pictures");

            FileItem resume = new FileItem("Resume.pdf");
            FileItem notes = new FileItem("Notes.txt");
            FileItem photo = new FileItem("Vacation.jpg");

            docs.Add(resume);
            docs.Add(notes);
            pics.Add(photo);

            root.Add(docs);
            root.Add(pics);
            root.Add(new FileItem("Readme.txt"));

            root.Display(0);
            
            Console.WriteLine();
        }
    }

    // Component
    public interface IFileSystemComponent
    {
        void Display(int indentLevel);
    }

    // Leaf
    public class FileItem : IFileSystemComponent
    {
        private string _name;

        public FileItem(string name)
        {
            _name = name;
        }

        public void Display(int indentLevel)
        {
            Console.WriteLine(new string('-', indentLevel) + " " + _name);
        }
    }

    // Composite
    public class FolderItem : IFileSystemComponent
    {
        private string _name;
        private List<IFileSystemComponent> _children = new List<IFileSystemComponent>();

        public FolderItem(string name)
        {
            _name = name;
        }

        public void Add(IFileSystemComponent component)
        {
            _children.Add(component);
        }

        public void Remove(IFileSystemComponent component)
        {
            _children.Remove(component);
        }

        public void Display(int indentLevel)
        {
            Console.WriteLine(new string('-', indentLevel) + "+ " + _name);
            foreach (var child in _children)
            {
                child.Display(indentLevel + 2);
            }
        }
    }
}
