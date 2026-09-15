# 📐 Software Design Principles

A hands-on C# / .NET 10 reference for **OOP fundamentals**, **SOLID principles**, and all **23 Gang-of-Four design patterns** — built for learning and interview revision.

Run the interactive console menu to explore each concept with clear, self-contained demos you can read, run, and debug.

---

## 📚 What's Inside

### OOP — Object-Oriented Programming
| # | Principle | Source |
|---|-----------|--------|
| 1 | Encapsulation | [`Encapsulation.cs`](OOP/Encapsulation.cs) |
| 2 | Inheritance | [`Inheritance.cs`](OOP/Inheritance.cs) |
| 3 | Polymorphism | [`Polymorphism.cs`](OOP/Polymorphism.cs) |
| 4 | Abstraction | [`Abstraction.cs`](OOP/Abstraction.cs) |

### SOLID Principles
| # | Principle | Source |
|---|-----------|--------|
| 1 | Single Responsibility (SRP) | [`SingleResponsibility.cs`](SOLID/SingleResponsibility.cs) |
| 2 | Open/Closed (OCP) | [`OpenClosed.cs`](SOLID/OpenClosed.cs) |
| 3 | Liskov Substitution (LSP) | [`LiskovSubstitution.cs`](SOLID/LiskovSubstitution.cs) |
| 4 | Interface Segregation (ISP) | [`InterfaceSegregation.cs`](SOLID/InterfaceSegregation.cs) |
| 5 | Dependency Inversion (DIP) | [`DependencyInversion.cs`](SOLID/DependencyInversion.cs) |

### Design Patterns — Creational
| # | Pattern | Source |
|---|---------|--------|
| 1 | Singleton | [`SingletonPattern.cs`](DesignPatterns/CreationalPatterns/SingletonPattern.cs) |
| 2 | Factory Method | [`FactoryMethodPattern.cs`](DesignPatterns/CreationalPatterns/FactoryMethodPattern.cs) |
| 3 | Abstract Factory | [`AbstractFactoryPattern.cs`](DesignPatterns/CreationalPatterns/AbstractFactoryPattern.cs) |
| 4 | Builder | [`BuilderPattern.cs`](DesignPatterns/CreationalPatterns/BuilderPattern.cs) |
| 5 | Prototype | [`PrototypePattern.cs`](DesignPatterns/CreationalPatterns/PrototypePattern.cs) |

### Design Patterns — Structural
| # | Pattern | Source |
|---|---------|--------|
| 1 | Adapter | [`AdapterPattern.cs`](DesignPatterns/StructuralPatterns/AdapterPattern.cs) |
| 2 | Bridge | [`BridgePattern.cs`](DesignPatterns/StructuralPatterns/BridgePattern.cs) |
| 3 | Composite | [`CompositePattern.cs`](DesignPatterns/StructuralPatterns/CompositePattern.cs) |
| 4 | Decorator | [`DecoratorPattern.cs`](DesignPatterns/StructuralPatterns/DecoratorPattern.cs) |
| 5 | Facade | [`FacadePattern.cs`](DesignPatterns/StructuralPatterns/FacadePattern.cs) |
| 6 | Flyweight | [`FlyweightPattern.cs`](DesignPatterns/StructuralPatterns/FlyweightPattern.cs) |
| 7 | Proxy | [`ProxyPattern.cs`](DesignPatterns/StructuralPatterns/ProxyPattern.cs) |

### Design Patterns — Behavioral
| # | Pattern | Source |
|---|---------|--------|
| 1 | Chain of Responsibility | [`ChainOfResponsibilityPattern.cs`](DesignPatterns/BehavioralPatterns/ChainOfResponsibilityPattern.cs) |
| 2 | Command | [`CommandPattern.cs`](DesignPatterns/BehavioralPatterns/CommandPattern.cs) |
| 3 | Iterator | [`IteratorPattern.cs`](DesignPatterns/BehavioralPatterns/IteratorPattern.cs) |
| 4 | Mediator | [`MediatorPattern.cs`](DesignPatterns/BehavioralPatterns/MediatorPattern.cs) |
| 5 | Memento | [`MementoPattern.cs`](DesignPatterns/BehavioralPatterns/MementoPattern.cs) |
| 6 | Observer | [`ObserverPattern.cs`](DesignPatterns/BehavioralPatterns/ObserverPattern.cs) |
| 7 | State | [`StatePattern.cs`](DesignPatterns/BehavioralPatterns/StatePattern.cs) |
| 8 | Strategy | [`StrategyPattern.cs`](DesignPatterns/BehavioralPatterns/StrategyPattern.cs) |
| 9 | Template Method | [`TemplateMethodPattern.cs`](DesignPatterns/BehavioralPatterns/TemplateMethodPattern.cs) |
| 10 | Visitor | [`VisitorPattern.cs`](DesignPatterns/BehavioralPatterns/VisitorPattern.cs) |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later

### Run

```bash
dotnet run
```

An interactive menu will appear — pick a number to run any demo, or press `0` to run them all sequentially.

---

## 🗂️ Project Structure

```
software-design-principles/
├── OOP/                          # 4 OOP pillars
├── SOLID/                        # 5 SOLID principles
├── DesignPatterns/
│   ├── CreationalPatterns/       # 5 patterns
│   ├── StructuralPatterns/       # 7 patterns
│   └── BehavioralPatterns/       # 10 patterns
├── Program.cs                    # Interactive console menu
└── SoftwareDesignPrinciples.csproj
```

---

## 🤝 Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request if you'd like to add more examples, improve explanations, or fix bugs.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).
