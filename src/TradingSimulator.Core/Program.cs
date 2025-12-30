using System;
using TradingSimulator.Core.Models;
using TradingSimulator.Core.Enums;

class Program
{
    static void Main()
    {
        // === CREATE BUY TRANSACTION ===
        var buyTransaction = new Transaction(
            EnumTransacitonType.Buy,
            "AAPL",
            10,
            150.50m
        );

        // === CREATE SELL TRANSACTION WITH CUSTOM TIME ===
        var sellTransaction = new Transaction(
            EnumTransacitonType.Sell,
            "TSLA",
            5,
            200.00m,
            new DateTime(2025, 1, 10, 14, 30, 0)
        );

        // === OUTPUT ===
        PrintTransaction(buyTransaction);
        Console.WriteLine();
        PrintTransaction(sellTransaction);

        // === DEBUG POINT ===
        Console.WriteLine("\nPress ENTER to exit...");
        Console.ReadLine();
    }

    static void PrintTransaction(Transaction tx)
    {
        Console.WriteLine(tx);
    }
}
