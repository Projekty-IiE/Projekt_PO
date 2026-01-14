using System;
using TradingSimulator.Core.Models;
using TradingSimulator.Core.Services;

class Program
{
    static void Main()
    {
        //// === CREATE BUY TRANSACTION ===
        //var buyTransaction = new Transaction(
        //    EnumTransacitonType.Buy,
        //    "AAPL",
        //    10,
        //    150.50m
        //);

        //// === CREATE SELL TRANSACTION WITH CUSTOM TIME ===
        //var sellTransaction = new Transaction(
        //    EnumTransacitonType.Sell,
        //    "TSLA",
        //    5,
        //    200.00m,
        //    new DateTime(2025, 1, 10, 14, 30, 0)
        //);

        //// === OUTPUT ===
        //PrintTransaction(buyTransaction);
        //Console.WriteLine();
        //PrintTransaction(sellTransaction);

        //// === DEBUG POINT ===
        //Console.WriteLine("\nPress ENTER to exit...");
        //Console.ReadLine();

        // === CREATE STOCKS ===
        var stocks = new List<Stock>
        {
            new Stock("AAPL", "Apple Inc.", 150m),
            new Stock("TSLA", "Tesla Inc.", 220m),
            new Stock("MSFT", "Microsoft", 310m)
        };

        // === CREATE MARKET ENGINE ===
        var market = new MarketEngine(stocks);

        Console.WriteLine("=== INITIAL PRICES ===");
        PrintPrices(market);

        // === SIMULATE MARKET TICKS ===
        for (int i = 1; i <= 5; i++)
        {
            Console.WriteLine($"\n=== TICK {i} ===");
            market.UpdateMarket(); //generating price changes
            PrintPrices(market);

            System.Threading.Thread.Sleep(1000); // pause for readability
        }

        Console.WriteLine("\nSimulation finished. Press ENTER to exit.");
        Console.ReadLine();
    }

    static void PrintTransaction(Transaction tx)
    {
        Console.WriteLine(tx);
    }

    static void PrintPrices(MarketEngine market)
    {
        foreach (var stock in market.Stocks)
        {
            Console.WriteLine($"{stock.Symbol} | {stock.Price}");
        }
    }
}
