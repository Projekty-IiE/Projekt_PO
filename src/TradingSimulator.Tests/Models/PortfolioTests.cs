using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradingSimulator.Core.Models;
using TradingSimulator.Core.Exceptions;

namespace TradingSimulator.Tests.Models
{
    [TestClass]
    public class PortfolioTests
    {
        [TestMethod]
        public void Constructor_InitializesCorrectly()
        {
            var portfolio = new Portfolio(10_000m);

            Assert.AreEqual(10_000m, portfolio.Balance);
            Assert.AreEqual(0, portfolio.Items.Count);
            Assert.AreEqual(0, portfolio.Transactions.Count);
            Assert.AreEqual(0m, portfolio.RealizedPnL);
        }

        [TestMethod]
        public void BuyStock_DecreasesBalance_AndAddsItem()
        {
            var portfolio = new Portfolio(10_000m);
            var stock = new Stock("AAPL", "Apple", 100m);

            portfolio.BuyStock(stock, 5);

            Assert.AreEqual(9_500m, portfolio.Balance);
            Assert.AreEqual(1, portfolio.Items.Count);
            Assert.AreEqual(1, portfolio.Transactions.Count);
        }

        [TestMethod]
        public void SellStock_IncreasesBalance_AndUpdatesPnL()
        {
            var portfolio = new Portfolio(10_000m);
            var stock = new Stock("AAPL", "Apple", 100m);

            portfolio.BuyStock(stock, 10); // -1000
            stock.Price = 120m;

            portfolio.SellStock(stock, 5); // +600

            Assert.AreEqual(9_600m, portfolio.Balance);
            Assert.AreEqual(100m, portfolio.RealizedPnL);
        }

        [TestMethod]
        public void SellStock_Throws_WhenSellingTooMuch()
        {
            var portfolio = new Portfolio(10_000m);
            var stock = new Stock("AAPL", "Apple", 100m);

            portfolio.BuyStock(stock, 2);

            Assert.ThrowsException<InsufficientSharesException>(() =>
                portfolio.SellStock(stock, 5));
        }
    }
}
