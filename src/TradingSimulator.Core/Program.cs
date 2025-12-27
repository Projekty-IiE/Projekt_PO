using TradingSimulator.Core.Models;

// Stock debug
Stock stock = new Stock("AAPL", "Apple Inc.", 0.5m);

//Portfolio Item debug

var item = new PortfolioItem(stock, 100);

stock.UpdatePrice(1m); // +100%

Console.WriteLine(item);