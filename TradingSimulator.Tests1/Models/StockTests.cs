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
    public class StockTests
    {
        [TestMethod]
        public void Constructor_InitializesStockCorrectly()
        {
            var stock = new Stock("AAPL", "Apple", 150m);

            Assert.AreEqual("AAPL", stock.Symbol);
            Assert.AreEqual(150m, stock.Price);
            Assert.AreEqual(1, stock.PriceHistory.Count); // ⚠️ konstruktor + setter
        }

        [TestMethod]
        public void UpdatePrice_AddsNewEntryToHistory()
        {
            var stock = new Stock("AAPL", "Apple", 100m);

            stock.UpdatePrice(0.1m);

            Assert.AreEqual(2, stock.PriceHistory.Count);
            Assert.AreNotEqual(0, stock.LastChange);
        }

        [TestMethod]
        public void SettingNegativePrice_Throws()
        {
            var stock = new Stock("AAPL", "Apple", 100m);

            Assert.ThrowsException<ArgumentException>(() =>
                stock.Price = -10m);
        }
    }
}

