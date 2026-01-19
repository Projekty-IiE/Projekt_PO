using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradingSimulator.Core.Services;
using TradingSimulator.Core.Models;
using TradingSimulator.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace TradingSimulator.Tests.Services
{
    [TestClass]
    public class PortfolioServiceTests
    {
        private Portfolio portfolio;
        private MarketEngine market;
        private PortfolioService service;

        [TestInitialize]
        public void Setup()
        {
            portfolio = new Portfolio(10_000m);

            var stocks = new List<Stock>
            {
                new Stock("AAPL", "Apple", 100m),
                new Stock("TSLA", "Tesla", 200m)
            };

            market = new MarketEngine(stocks);
            service = new PortfolioService(portfolio, market);
        }

        //CONSTRUCTOR

        [TestMethod]
        public void Constructor_Throws_WhenPortfolioIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new PortfolioService(null, market));
        }

        [TestMethod]
        public void Constructor_Throws_WhenMarketServiceIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new PortfolioService(portfolio, null));
        }

        //BUY

        [TestMethod]
        public void Buy_DecreasesBalance_AndAddsItem()
        {
            var tx = service.Buy("AAPL", 5);

            Assert.AreEqual(9_500m, service.Balance);
            Assert.AreEqual(1, service.Items.Count);
            Assert.AreEqual("AAPL", tx.StockSymbol);
            Assert.AreEqual(1, service.Transactions.Count);
        }

        [TestMethod]
        public void Buy_Throws_WhenSymbolIsEmpty()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                service.Buy("", 1));
        }

        [TestMethod]
        public void Buy_Throws_WhenStockNotFound()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                service.Buy("NVDA", 1));
        }

        [TestMethod]
        public void Buy_Throws_WhenInsufficientFunds()
        {
            Assert.ThrowsException<InsufficientFundsException>(() =>
                service.Buy("TSLA", 100));
        }

        //SELL

        [TestMethod]
        public void Sell_IncreasesBalance_AndUpdatesRealizedPnL()
        {
            service.Buy("AAPL", 10); // -1000
            var stock = market.Stocks[0];
            stock.Price = 120m;

            service.Sell("AAPL", 5);

            Assert.AreEqual(9_600m, service.Balance);
            Assert.AreEqual(100m, service.RealizedPnL);
            Assert.AreEqual(2, service.Transactions.Count);
        }

        [TestMethod]
        public void Sell_Throws_WhenStockNotOwned()
        {
            Assert.ThrowsException<InsufficientSharesException>(() =>
                service.Sell("AAPL", 1));
        }

        //TOTAL VALUE

        [TestMethod]
        public void TotalValue_ReflectsCashAndMarketValue()
        {
            service.Buy("AAPL", 5); // cash: 9500

            var stock = market.Stocks[0];
            stock.Price = 200m;

            Assert.AreEqual(10_500m, service.TotalValue);
        }

        //LOAD PORTFOLIO

        [TestMethod]
        public void LoadPortfolio_RestoresPortfolioAndMarketState()
        {
            var savedStock = new Stock("AAPL", "Apple", 150m);
            savedStock.PriceHistory.Add(140m);

            var item = new PortfolioItem(savedStock, 5, 150m);
            var tx = new BuyTransaction("AAPL", 5, 150m, DateTime.Now);

            service.LoadPortfolio(
                balance: 5_000m,
                realizedPnL: 200m,
                items: new List<PortfolioItem> { item },
                transactions: new List<Transaction> { tx },
                marketData: new List<Stock> { savedStock }
            );

            Assert.AreEqual(5_000m, service.Balance);
            Assert.AreEqual(200m, service.RealizedPnL);
            Assert.AreEqual(1, service.Items.Count);
            Assert.AreEqual(1, service.Transactions.Count);
            Assert.AreEqual(150m, market.Stocks[0].Price);
        }
    }
}
