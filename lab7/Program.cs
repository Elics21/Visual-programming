using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Reactive.Linq;

public static class ObservableCollectionFactory
{
    public static IObservable<NotifyCollectionChangedEventArgs> CreateObservable<T>(ObservableCollection<T> collection)
    {
        return Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
            handler => collection.CollectionChanged += handler,
            handler => collection.CollectionChanged -= handler)
            .Select(args => args.EventArgs);
    }

    public static IObservable<NotifyCollectionChangedEventArgs> CreateObservable(ObservableCollection<string> collection)
    {
        return CreateObservable<string>(collection);
    }
}

class Program
{
    static void Main(string[] args)
    {
        var collection = new ObservableCollection<string> { "Hello", "World", "Hi", "IP215" };
        var observable = ObservableCollectionFactory.CreateObservable(collection);

        observable.Subscribe(LogCollectionChanges);

        collection.Add("TOP");
        collection.RemoveAt(2);
        collection[0] = "Hi";
        Console.WriteLine();
        for (int i = 0; i < collection.Count; i++)
        {
            Console.WriteLine($"index: {i}, {collection[i]}");
        }

    }

    static void LogCollectionChanges(NotifyCollectionChangedEventArgs args)
    {
        var logMessage = $"Колекция изменилась, метод {args.Action}, старый индекс {args.OldStartingIndex}, новый индекс {args.NewStartingIndex}";
        Console.WriteLine(logMessage);  
        string logFilePath = AppDomain.CurrentDomain.BaseDirectory;
        //File.AppendAllText(Path.Combine(logFilePath, "log.txt"), logMessage + Environment.NewLine);
    }
}