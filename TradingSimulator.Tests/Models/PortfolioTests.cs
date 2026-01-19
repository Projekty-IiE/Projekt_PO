using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.Core.Models;
using TradingSimulator.Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradingSimulator.Tests.Models
{
    [TestClass]
    public class PortfolioTests
    {
        [TestMethod]
        public void Constructor_InitializesPortfolioCorrectly()
        {
            var portfolio = new Portfolio(10_000m);

            Assert.AreEqual(10_000m, portfolio.Balance);
            Assert.AreEqual(0, portfolio.Items.Count);
            Assert.AreEqual(0, portfolio.Transactions.Count);
            Assert.AreEqual(0m, portfolio.RealizedPnL);
        }

        [TestMethod]
        public void Constructor_Throws_WhenInitialBalanceIsNegative()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Portfolio(-1m));
        }

        [TestMethod]
        public void BuyStock_DecreasesBalance_AddsItemAndTransaction()
        {
            var portfolio = new Portfolio(10_000m);
            var stock = new Stock("AAPL", "Apple", 100m);

            var tx = portfolio.BuyStock(stock, 5);

            Assert.AreEqual(9_500m, portfolio.Balance);
            Assert.AreEqual(1, portfolio.Items.Count);
            Assert.AreEqual(1, portfolio.Transactions.Count);
            Assert.AreEqual("AAPL", tx.StockSymbol);
        }

        [TestMethod]
        public void BuyStock_Throws_WhenInsufficientFunds()
        {
            var portfolio = new Portfolio(100m);
            var stock = new Stock("AAPL", "Apple", 200m);

            Assert.ThrowsException<InsufficientFundsException>(() =>
                portfolio.BuyStock(stock, 1));
        }

        [TestMethod]
        public void SellStock_IncreasesBalance_AndUpdatesRealizedPnL()
        {
            var portfolio = new Portfolio(10_000m);
            var stock = new Stock("AAPL", "Apple", 100m);

            portfolio.BuyStock(stock, 10); // koszt 1000
            stock.Price = 120m;

            portfolio.SellStock(stock, 5);

            Assert.AreEqual(9_600m, portfolio.Balance);
            Assert.AreEqual(100m, portfolio.RealizedPnL); // (120 - 100) * 5
            Assert.AreEqual(2, portfolio.Transactions.Count);
        }

        [TestMethod]
        public void SellStock_RemovesItem_WhenQuantityReachesZero()
        {
            var portfolio = new Portfolio(10_000m);
            var stock = new Stock("AAPL", "Apple", 100m);

            portfolio.BuyStock(stock, 5);
            portfolio.SellStock(stock, 5);

            Assert.AreEqual(0, portfolio.Items.Count);
        }

        [TestMethod]
        public void SellStock_Throws_WhenSellingMoreThanOwned()
        {
            var portfolio = new Portfolio(10_000m);
            var stock = new Stock("AAPL", "Apple", 100m);

            portfolio.BuyStock(stock, 2);

            Assert.ThrowsException<InsufficientSharesException>(() =>
                portfolio.SellStock(stock, 5));
        }

        [TestMethod]
        public void LoadPortfolio_RestoresStateCorrectly()
        {
            var portfolio = new Portfolio(10_000m);
            var stock = new Stock("AAPL", "Apple", 100m);

            var item = new PortfolioItem(stock, 5, 100m);
            var tx = new BuyTransaction("AAPL", 5, 100m, DateTime.Now);

            portfolio.LoadPortfolio(
                newBalance: 5_000m,
                newPnL: 200m,
                newItems: new[] { item },
                history: new[] { tx });

            Assert.AreEqual(5_000m, portfolio.Balance);
            Assert.AreEqual(200m, portfolio.RealizedPnL);
            Assert.AreEqual(1, portfolio.Items.Count);
            Assert.AreEqual(1, portfolio.Transactions.Count);
        }
    }
}