using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.DesignPatterns.BehavioralPatterns
{
    /// <summary>
    /// Command Pattern: Encapsulates a request as an object, thereby letting you parameterize clients with different requests,
    /// queue or log requests, and support undoable operations.
    /// </summary>
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public class TextEditor
    {
        public string Text { get; set; } = "";

        public void TypeText(string text)
        {
            Text += text;
            Console.WriteLine($"Editor currently says: {Text}");
        }

        public void DeleteText(int length)
        {
            if (length > Text.Length) length = Text.Length;
            Text = Text.Substring(0, Text.Length - length);
            Console.WriteLine($"Editor currently says: {Text}");
        }
    }

    public class TypeCommand : ICommand
    {
        private readonly TextEditor _editor;
        private readonly string _textToType;

        public TypeCommand(TextEditor editor, string textToType)
        {
            _editor = editor;
            _textToType = textToType;
        }

        public void Execute()
        {
            Console.WriteLine($"Executing Type: '{_textToType}'");
            _editor.TypeText(_textToType);
        }

        public void Undo()
        {
            Console.WriteLine($"Undoing Type: '{_textToType}'");
            _editor.DeleteText(_textToType.Length);
        }
    }

    public class DeleteCommand : ICommand
    {
        private readonly TextEditor _editor;
        private readonly int _length;
        private string _deletedText = "";

        public DeleteCommand(TextEditor editor, int length)
        {
            _editor = editor;
            _length = length;
        }

        public void Execute()
        {
            Console.WriteLine($"Executing Delete: {_length} characters");
            if (_length > _editor.Text.Length)
            {
                _deletedText = _editor.Text;
            }
            else
            {
                _deletedText = _editor.Text.Substring(_editor.Text.Length - _length);
            }
            _editor.DeleteText(_length);
        }

        public void Undo()
        {
            Console.WriteLine($"Undoing Delete: Restoring '{_deletedText}'");
            _editor.TypeText(_deletedText);
        }
    }

    public class CommandHistory
    {
        private readonly Stack<ICommand> _history = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _history.Push(command);
        }

        public void Undo()
        {
            if (_history.Count > 0)
            {
                var command = _history.Pop();
                command.Undo();
            }
            else
            {
                Console.WriteLine("Nothing to undo.");
            }
        }
    }

    public static class CommandPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Command Pattern Demo ---");
            Console.WriteLine("Theme: Text editor with undo/redo");

            var editor = new TextEditor();
            var history = new CommandHistory();

            history.ExecuteCommand(new TypeCommand(editor, "Hello "));
            history.ExecuteCommand(new TypeCommand(editor, "World!"));
            history.ExecuteCommand(new DeleteCommand(editor, 6)); // Delete "World!"
            history.ExecuteCommand(new TypeCommand(editor, "Design Patterns."));

            Console.WriteLine("\n--- Undoing ---");
            history.Undo(); // Undo Type "Design Patterns."
            history.Undo(); // Undo Delete
            history.Undo(); // Undo Type "World!"
            history.Undo(); // Undo Type "Hello "

            Console.WriteLine("----------------------------\n");
        }
    }
}
