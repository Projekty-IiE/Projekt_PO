using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Tests.Models
{
    [TestClass]
    public class TransactionTests
    {
        [TestMethod]
        public void BuyTransaction_CreatesCorrectly()
        {
            var tx = new BuyTransaction(
                "AAPL",
                5,
                100m,
                DateTime.Now
            );

            Assert.AreEqual("AAPL", tx.StockSymbol);
            Assert.AreEqual(5, tx.Quantity);
            Assert.AreEqual(100m, tx.PricePerShare);
            Assert.AreEqual(500m, tx.TotalValue);
        }

        [TestMethod]
        public void SellTransaction_CreatesCorrectly()
        {
            var tx = new SellTransaction(
                "AAPL",
                5,
                120m,
                100m,
                DateTime.Now
            );

            Assert.AreEqual("AAPL", tx.StockSymbol);
            Assert.AreEqual(5, tx.Quantity);
            Assert.AreEqual(120m, tx.PricePerShare);
            Assert.AreEqual(600m, tx.TotalValue);
            Assert.AreEqual(100m, tx.RealizedPnL);
        }
    }
}
