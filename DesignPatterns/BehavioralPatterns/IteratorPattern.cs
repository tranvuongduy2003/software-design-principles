using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareDesignPrinciples.DesignPatterns.BehavioralPatterns
{
    /// <summary>
    /// Iterator Pattern: Provides a way to access the elements of an aggregate object sequentially
    /// without exposing its underlying representation.
    /// </summary>
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
    }

    public interface IIterableCollection<T>
    {
        IIterator<T> CreateIterator();
        IIterator<T> CreateAuthorIterator();
    }

    public class Book
    {
        public string Title { get; }
        public string Author { get; }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }

        public override string ToString() => $"'{Title}' by {Author}";
    }

    public class BookShelf : IIterableCollection<Book>
    {
        private readonly List<Book> _books = new List<Book>();

        public void AddBook(Book book) => _books.Add(book);

        public List<Book> GetBooks() => _books;

        public IIterator<Book> CreateIterator()
        {
            return new TitleIterator(this);
        }

        public IIterator<Book> CreateAuthorIterator()
        {
            return new AuthorIterator(this);
        }
    }

    public class TitleIterator : IIterator<Book>
    {
        private readonly List<Book> _books;
        private int _position = 0;

        public TitleIterator(BookShelf bookShelf)
        {
            _books = bookShelf.GetBooks().OrderBy(b => b.Title).ToList();
        }

        public bool HasNext() => _position < _books.Count;

        public Book Next() => _books[_position++];
    }

    public class AuthorIterator : IIterator<Book>
    {
        private readonly List<Book> _books;
        private int _position = 0;

        public AuthorIterator(BookShelf bookShelf)
        {
            _books = bookShelf.GetBooks().OrderBy(b => b.Author).ToList();
        }

        public bool HasNext() => _position < _books.Count;

        public Book Next() => _books[_position++];
    }

    public static class IteratorPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Iterator Pattern Demo ---");
            Console.WriteLine("Theme: Custom collection (BookShelf) iterated in different orders");

            var shelf = new BookShelf();
            shelf.AddBook(new Book("The Hobbit", "Tolkien"));
            shelf.AddBook(new Book("1984", "Orwell"));
            shelf.AddBook(new Book("Dune", "Herbert"));
            shelf.AddBook(new Book("Foundation", "Asimov"));

            Console.WriteLine("\nIterating by Title:");
            var titleIterator = shelf.CreateIterator();
            while (titleIterator.HasNext())
            {
                Console.WriteLine($" - {titleIterator.Next()}");
            }

            Console.WriteLine("\nIterating by Author:");
            var authorIterator = shelf.CreateAuthorIterator();
            while (authorIterator.HasNext())
            {
                Console.WriteLine($" - {authorIterator.Next()}");
            }

            Console.WriteLine("-----------------------------\n");
        }
    }
}
