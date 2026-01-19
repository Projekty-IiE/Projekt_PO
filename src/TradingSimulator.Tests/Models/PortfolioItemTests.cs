using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradingSimulator.Tests.Models
{
    [TestClass]
    public class PortfolioItemTests
    {
        [TestMethod]
        public void Constructor_CreatesItemCorrectly()
        {
            var stock = new Stock("AAPL", "Apple", 100m);

            var item = new PortfolioItem(stock, 5, 100m);

            Assert.AreEqual(5, item.Quantity);
            Assert.AreEqual(100m, item.AveragePrice);
            Assert.AreEqual(stock, item.Stock);
        }

        [TestMethod]
        public void Add_IncreasesQuantity_AndRecalculatesAveragePrice()
        {
            var stock = new Stock("AAPL", "Apple", 100m);
            var item = new PortfolioItem(stock, 5, 100m);

            item.Add(5, 120m);

            Assert.AreEqual(10, item.Quantity);
            Assert.AreEqual(110m, item.AveragePrice);
        }

        [TestMethod]
        public void Remove_DecreasesQuantity()
        {
            var stock = new Stock("AAPL", "Apple", 100m);
            var item = new PortfolioItem(stock, 5, 100m);

            item.Remove(2);

            Assert.AreEqual(3, item.Quantity);
        }

        [TestMethod]
        public void Remove_Throws_WhenRemovingTooMuch()
        {
            var stock = new Stock("AAPL", "Apple", 100m);
            var item = new PortfolioItem(stock, 2, 100m);

            Assert.ThrowsException<InvalidOperationException>(() =>
                item.Remove(5));
        }
    }
}
