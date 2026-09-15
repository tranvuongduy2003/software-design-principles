using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.DesignPatterns.BehavioralPatterns
{
    /// <summary>
    /// Mediator Pattern: Defines an object that encapsulates how a set of objects interact.
    /// Mediator promotes loose coupling by keeping objects from referring to each other explicitly.
    /// </summary>
    public interface IChatMediator
    {
        void SendMessage(string message, User sender);
        void RegisterUser(User user);
    }

    public abstract class User
    {
        protected IChatMediator _mediator;
        public string Name { get; }

        public User(IChatMediator mediator, string name)
        {
            _mediator = mediator;
            Name = name;
        }

        public abstract void Send(string message);
        public abstract void Receive(string message, string senderName);
    }

    public class ChatRoom : IChatMediator
    {
        private readonly List<User> _users = new List<User>();

        public void RegisterUser(User user)
        {
            _users.Add(user);
        }

        public void SendMessage(string message, User sender)
        {
            foreach (var user in _users)
            {
                // Don't send message to the sender
                if (user != sender)
                {
                    user.Receive(message, sender.Name);
                }
            }
        }
    }

    public class ChatUser : User
    {
        public ChatUser(IChatMediator mediator, string name) : base(mediator, name)
        {
        }

        public override void Send(string message)
        {
            Console.WriteLine($"[{Name}] sending msg: {message}");
            _mediator.SendMessage(message, this);
        }

        public override void Receive(string message, string senderName)
        {
            Console.WriteLine($"[{Name}] received from [{senderName}]: {message}");
        }
    }

    public static class MediatorPatternDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- Mediator Pattern Demo ---");
            Console.WriteLine("Theme: Chat room routing messages");

            IChatMediator chatRoom = new ChatRoom();

            User user1 = new ChatUser(chatRoom, "Alice");
            User user2 = new ChatUser(chatRoom, "Bob");
            User user3 = new ChatUser(chatRoom, "Charlie");

            chatRoom.RegisterUser(user1);
            chatRoom.RegisterUser(user2);
            chatRoom.RegisterUser(user3);

            user1.Send("Hello everyone!");
            Console.WriteLine();
            user2.Send("Hey Alice!");
            Console.WriteLine();
            user3.Send("Morning!");

            Console.WriteLine("-----------------------------\n");
        }
    }
}
