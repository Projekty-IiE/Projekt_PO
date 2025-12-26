using TradingSimulator.Core.Models;

// Stock debug
var stock = new Stock("AAPL", "Apple Inc.", 0.5m);
stock.UpdatePrice(0.03m);   // +3%
stock.UpdatePrice(-0.02m);  // -2%
Console.WriteLine(stock);