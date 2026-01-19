using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradingSimulator.Core.Services;
using TradingSimulator.Core.Models;
using System;
using System.Collections.Generic;

namespace TradingSimulator.Tests.Services
{
    [TestClass]
    public class MarketEngineTests
    {
        [TestMethod]
        public void Constructor_StoresStocksCorrectly()
        {
            var stocks = new List<Stock>
            {
                new Stock("AAPL", "Apple", 100m),
                new Stock("TSLA", "Tesla", 200m)
            };

            var engine = new MarketEngine(stocks);

            Assert.AreEqual(2, engine.Stocks.Count);
        }

        [TestMethod]
        public void Constructor_Throws_WhenStocksIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new MarketEngine(null));
        }

        [TestMethod]
        public void UpdateMarket_UpdatesStockPrices()
        {
            var stock = new Stock("AAPL", "Apple", 100m);
            var engine = new MarketEngine(new[] { stock });

            var oldPrice = stock.Price;
            var oldHistoryCount = stock.PriceHistory.Count;

            engine.UpdateMarket();

            Assert.AreEqual(oldHistoryCount + 1, stock.PriceHistory.Count);
            Assert.AreNotEqual(0m, stock.LastChange);
            Assert.AreNotEqual(oldPrice, stock.Price);
        }

        [TestMethod]
        public void UpdateMarket_UpdatesAllStocks()
        {
            var stocks = new List<Stock>
            {
                new Stock("AAPL", "Apple", 100m),
                new Stock("MSFT", "Microsoft", 150m),
                new Stock("TSLA", "Tesla", 200m)
            };

            var engine = new MarketEngine(stocks);

            engine.UpdateMarket();

            foreach (var stock in stocks)
            {
                Assert.AreEqual(2, stock.PriceHistory.Count);
            }
        }
    }
}
